namespace TestServer
{
    class SpinLock
    {
        volatile int _locked = 0; 
        public void Acquire()
        {
            while(true)
            {
                // Exchange를 이용한 원자적 연산
                /* 대충 original 값하고 locked를 비교해서 중첩 확인 */
                //int original = Interlocked.Exchange(ref _locked, 1);
                //if (original == 0)
                //{
                //    break;
                //}

                // ComapreExchange : 1번과 3번 인자를 비교해서 둘이 같으면 2번 값을 넣음
                // CAS (compare and swap)
                int original = Interlocked.CompareExchange(ref _locked, 1, 0);
                if (original == 0)
                {
                    break;
                }

            }
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
