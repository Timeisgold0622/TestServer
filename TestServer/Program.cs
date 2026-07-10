namespace TestServer
{
    class Program
    {
        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < number; i++)
            {

                lock (_obj) // 아래랑 같은 코드인데 이제 잠금을 못푸는 경우를 방지해주는 구문
                {
                    number++;
                }
                // 상호 배체 Mutual Exclusive

                // CriticalSection, std::mutex
                Monitor.Enter(_obj); // 문을 잠구는 행위
                number++;
                Monitor.Exit(_obj); // 잠금을 풀어줌
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < number; i++)
            {
                Monitor.Enter(_obj);
                number--;
                Monitor.Exit(_obj);
            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
}
