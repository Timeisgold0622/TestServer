namespace TestServer
{
    class Program
    {
        static int number = 0;

        static void Thread_1()
        {
            for (int i = 0; i < number; i++)
            {
                Interlocked.Increment(ref number); // 1
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < number; i++)
            {
                Interlocked.Decrement(ref number); // 0 
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
