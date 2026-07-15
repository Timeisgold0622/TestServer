using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    internal class Session
    {
        Socket _socket;
        int _disconnected = 0; // disconnect 연속 호출 방지용

        public void Start(Socket socket)
        {
            _socket = socket;
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            // recvArgs.UserToken 는 추가적으로 넣고 싶은 내용 있으면 넣는 거, object 형식이라 다 넣기 가능
            // 버퍼 만들기
            recvArgs.SetBuffer(new byte[1024],0,1024);

            RegisterRecv(recvArgs);
        }

        public void Send(byte[] sendBuff)
        {
            _socket.Send(sendBuff);
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)// 멀티스레드 환경이기 때문에 레이스컨디션 방지용
                return;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        #region 네트워크 통신
        void RegisterRecv(SocketAsyncEventArgs args)
        {
            bool pending = _socket.ReceiveAsync(args);
            if (pending == false)
            {
                OnRecvCompleted(null, args);
            }
        }

        void OnRecvCompleted(object sender,SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success) // 상대가 끊거나 뭐 할때 가끔씩 0으로 옴
            {
                try
                {
                    string recvdata = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred); // Offset : 시작 위치, ByteTransferred : 몇 바이트 받았냐
                    Console.WriteLine($"[From Server] : {recvdata}");

                    RegisterRecv(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e}");
                }
            }
            else
            {
                Disconnect();
            }
        }
        #endregion

    }
}
