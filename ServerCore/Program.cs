using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    internal class Program
    {
        static Listener _listener = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                Session session = new Session();
                session.Start(clientSocket);

                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to ERTServer");
                session.Send(sendBuff);

                Thread.Sleep(1000);
                session.Disconnect(); // 쫒아내기
            }
            catch (Exception e)
            {
                    Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            // DNS를 이용해 구현하겠음
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0]; // AddressList가 리스트인 이유가 서버 부하를 막기 위해 여러 주소를 사용할 수도 있기 때문인데,
                                                      // 어짜피 우린 한 주소만 사용할 거라서 1번 주소를 쓸거
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8080); // ip의 최종 주소임, 두번째 인자는 포트 값
            // GetHostEntry를 통해 DNS 주소를 아이피 주소로 찾음

            _listener.init(endPoint,OnAcceptHandler);
            while (true)
            {
                    ;
            }
        }
    }
}
