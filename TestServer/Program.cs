namespace TestServer
{
    class Lock
    {
        // bool <- 커널
        ManualResetEvent _available = new ManualResetEvent(true);
        public void Acquire()
        {
            _available.WaitOne(); // 입장 시도
            _available.Reset(); // 자동으로 안해줘서 수동으로 닫아야됨
            // 근데 이러면 연산이 두개로 분리되서 Race Condition이 나타남 그렇기 떄문에 ManualResetEvent는 여러 개의 스레드를 사용해야될때 쓴다고 볼 수 있음
        }
        public void Release()
        {
            _available.Set(); // flag = true
        }
    }
    class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();

        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_1);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
}
