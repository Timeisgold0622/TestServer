namespace TestServer
{
    class Program
    {
        class Reward
        {

        }

        // RWLock, RenderWriteLock
        static ReaderWriterLockSlim _lock3 = new ReaderWriterLockSlim(); // Slim 붙은게 더 최신 버전

        static Reward GetRewardByid(int id)
        {
            _lock3.EnterReadLock();

            _lock3.ExitReadLock();
            return null;
        }

        static void AddReward(Reward reward)
        {
            _lock3.EnterWriteLock();

            _lock3.ExitWriteLock();
        }
        static void Main(string[] args)
        {
            
        }
    }
}
