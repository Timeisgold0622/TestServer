namespace TestServer
{
    class Program
    {
        static int _num = 0;
        static Mutex _lock = new Mutex();
        // Mutex 또한 커널 동기화 개체임
        // Mutex는 여러 종류의 데이터를 가지고 있어서 ResetEvent들과는 다름
        // 대표적으로 락 횟수와 스레드 아이디 가지고, 릴리스 에러도 잡음

        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.WaitOne();
                _num++;
                _lock.ReleaseMutex();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.WaitOne();
                _num--;
                _lock.ReleaseMutex();
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
