using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    internal class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        public void init(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 한번만 만들어 놓아도 재사용 가능
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted); // 이벤트 핸들러는 비동기로 새 스레드를 만들어서 호출 시킴(중요)
            RegisterAccept(args); // 초기화 시점에서 등록
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // 여기서 소캣에이싱크이벤트알규먼트를 초기화하는 것이 중요함
            args.AcceptSocket = null;
            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
            {
                OnAcceptCompleted(null, args);
            }
        }
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }

            RegisterAccept(args); // 다음 순번 등록
        }
    }
}
