using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // DNS를 이용해 구현하겠음
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0]; // AddressList가 리스트인 이유가 서버 부하를 막기 위해 여러 주소를 사용할 수도 있기 때문인데,
                                                      // 어짜피 우린 한 주소만 사용할 거라서 1번 주소를 쓸거
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8080); // ip의 최종 주소임, 두번째 인자는 포트 값
            // GetHostEntry를 통해 DNS 주소를 아이피 주소로 찾음

            // 문지기 만들기
            // AddressFamily는 ipv4 인지 ipv6를 사용할 것인지 설정하는 건데, 이미 DNS에서 설정되어 넣어져 있으니까 그냥 넣으면 됨
            // 그리고 tcp 프로토콜을 사용할 경우 SocketType을 Stream과 ProtocolType을 Tcp로 설정해야됨
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 문지기 교육
                listenSocket.Bind(endPoint); // ip 동기화라고 보면 됨

                // 영업시간
                // backlog : 최대 대기 수
                listenSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("Listening....");

                    // 손님을 입장시킨다
                    Socket clientSocket = listenSocket.Accept(); // 여기서 반환되는 값이 대리인(세션)의 소켓임
                    // 그리고 위 함수는 블로킹 함수라서, 위 함수가 실행이 원활히 되지 않으면 아랫단계 내용이 실행이 안됨
                    // 받는다.
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    string recvDatas = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); // 데이터가 중간부터 데이터를 받을 수도 있지만 지금은 그런 경우가 없기 때문에 0부터 받음
                    Console.WriteLine($"[from Client] : {recvDatas}");

                    // 보낸다.
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to ERTServer");
                    clientSocket.Send(sendBuff);

                    // 쫒아낸다.
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close(); // 이거만 해도 되지만, 명시해주기 위해서 미리 셧다운을 호출해주기
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString();
            }
        }
    }
}
