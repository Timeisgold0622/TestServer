using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8080);

            // 휴대폰 설정
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 문지기에게 요청 보내기
                socket.Connect(endPoint); // 얘도 블로킹함수라, 문지기가 안 받아주면 대기함
                Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}"); //  socket.RemoteEndPoint은 연결한 대상임

                // 보낸다.
                byte[] sendbuff = Encoding.UTF8.GetBytes("Hello World!");
                int sendBytes = socket.Send(sendbuff);

                // 받는다.
                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"[From Server] : {recvData}");

                // 나간다.
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
