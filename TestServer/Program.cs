namespace TestServer
{
    class SpinLock
    {
        volatile bool _locked = false; // 가시성
        public void Acquire()
        {
            while (_locked)
            {
                // 잠김이 풀리기를 기다림
            }

            _locked = true;
        }
        public void Release()
        {
            _locked = false;
        }
    }
    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < _num; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < _num; i++)
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
