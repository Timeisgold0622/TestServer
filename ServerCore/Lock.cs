using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    class Lock
    {
        // 재귀적 락을 허용할지 (yes) WriteLock OK, WriteLock->ReadLock OK, ReadLock->WriteLock NO
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int Max_Spin_Count = 5000;

        // [Unused(1)] [WriteThreadId (15)] [ReadCount (16)]
        int _flag = EMPTY_FLAG;
        int _writeCount = 0;

        public void WriteLock()
        {
            // 동일 스레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                _writeCount++;
                return;
            }
            // 아무도 WriteLock or ReadLock을 획득하고 있지 않을때, 경합해서 소유권을 얻음
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while (true)
            {
                for (int i = 0; i < Max_Spin_Count; i++ )
                {
                    if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;
                        return;
                    }
                }

                Thread.Yield();
            }
        }
        public void WriteUnLock()
        {
            int lockCount = --_writeCount;
            if (lockCount == 0)
            {
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
            }
        }
        public void ReadLock()
        {
            // 동일 스레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;
            }

            // 아무도 WriteLock을 획득하고 있지 않으면, ReadCount를 1늘린다.
            while (true)
            {
                for (int i = 0; i < Max_Spin_Count; i++)
                {
                    // 여기 이해될떄까지 공부
                    int expected = (_flag % READ_MASK);
                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                        return;
                }

                Thread.Yield();
            }

        }
        public void ReadUnLock()
        {
            Interlocked.Decrement(ref _flag);
        }
    }
}
