namespace TestServer
{
    class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 10; i++) {
                Console.WriteLine("Hello, Thread!");
            }
        }
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1); // 최소 스레드 수
            ThreadPool.SetMaxThreads(5, 5); // 최대 스레드 수

            Task t = new Task(() => {while (true) { } }, TaskCreationOptions.LongRunning); // LongRunning 인자로 인해서 먹통되지 않음.

            ThreadPool.QueueUserWorkItem(MainThread); // 필요할때 호출하는 Thread + 갯수 제한 있음
        }
    }
}
