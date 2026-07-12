namespace ServerCore
{
    internal class Program
    {
        // 스레드마다 고유한 자원 부여
        // static ThreadLocal<string> ThreadName = new ThreadLocal<string>();
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => {return $"My Name is {Thread.CurrentThread.ManagedThreadId}";});
        // 위 코드는 자동으로 ThreadName이 할당된 스레드면 새로 부여 안하고, 안되었으면 해당 return 값을 부여함


        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated; // 이 스레드가 이미 만들어졌으면 True, 아니면 False
            if (repeat)
            {
                Console.WriteLine(ThreadName.Value + " (repeat)");
            }
            else
            {
                Console.WriteLine(ThreadName.Value); // 이때 위의 있는 new ThreadLocal<string>(() => {return $"My Name is {Thread.CurrentThread.ManagedThreadId}";})가 실행됨 
            }
        }
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            ThreadName.Dispose(); // 데이터 날리기
        }
    }
}
