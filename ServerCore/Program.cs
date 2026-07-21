using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace ServerCore
{
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Connected : {endPoint}");

            byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to ERTServer");
            Send(sendBuff);

            Thread.Sleep(1000);
            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Disconnected : {endPoint}");
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvdata = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count); // Offset : 시작 위치, ByteTransferred : 몇 바이트 받았냐
            Console.WriteLine($"[From Server] : {recvdata}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
    internal class Program
    {
        static Listener _listener = new Listener();
        
        static void Main(string[] args)
        {
            // DNS를 이용해 구현하겠음
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0]; // AddressList가 리스트인 이유가 서버 부하를 막기 위해 여러 주소를 사용할 수도 있기 때문인데,
                                                      // 어짜피 우린 한 주소만 사용할 거라서 1번 주소를 쓸거
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8080); // ip의 최종 주소임, 두번째 인자는 포트 값
            // GetHostEntry를 통해 DNS 주소를 아이피 주소로 찾음

            _listener.init(endPoint, () => { return new GameSession(); });
            while (true)
            {
                    ;
            }
        }
    }
}
