namespace TestServer
{
    class Program
    {
        static object _lock = new object();
        static SpinLock _lock2 = new SpinLock();
        // C#에서 제공하는 SpinLock은 기본적으로 근성 즉, 자기가 계속 스레드를 쓰긴 하지만
        // 도저히 답이 없는 거 같으면 양보를 하기도 함
        static void Main(string[] args)
        {
            lock (_lock)
            {

            }

            bool lockTaken = false;

            try
            {
                _lock2.Enter(ref lockTaken);
            }
            finally
            {
                if (lockTaken){
                    _lock2.Exit();
                }
            }
        }
    }
}
