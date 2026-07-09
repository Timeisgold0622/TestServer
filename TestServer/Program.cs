namespace TestServer
{
    class Program
    {
        volatile static bool _stop = false; // volatile : 잘 변할 수 있는 얘 니까 컴파일러보고 관련된거 최적화 하지 말라한거

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");
            while (_stop == false)
            {
                // 누군가가 stop 신호를 해주길 기다린다.
            }
            Console.WriteLine("쓰레드 종료");
        }
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000); // 1초 동안 중단했다가 다시 실행됨

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기 중");
            t.Wait(); // task가 끝남을 알림 (join과 같음)
            Console.WriteLine("종료 성공");
        }
    }
}
