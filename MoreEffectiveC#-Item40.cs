using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace LockKeyWord
{

    /**
Item40: Use lock() as Your First Choice for
Synchronization

lock (x)
{
        // Your code...
}

object __lockObj = x;
bool __lockWasTaken = false;
try
{
        System.Threading.Monitor.Enter(__lockObj, ref __lockWasTaken);
        // Your code...
}
finally
{
        if (__lockWasTaken) System.Threading.Monitor.Exit(__lockObj);
}


https://docs.microsoft.com/en-us/dotnet/api/system.threading.interlocked.increment?view=net-6.0
    **/


    public class LockKeywordTest
    {
        [Fact]
        public async Task GivenTotalWithMonitor_WhenUseValueTypeLock_ThenGetException()
        {
            var totalObject = new TotalWithMonitor();

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => totalObject.IncrementTotal()));
            }
            await Task.WhenAll(tasks);

            Assert.Equal(10, totalObject.TotalValue);
        }

        [Fact]
        public async Task GivenTotal_WhenUseRefTypeLockAndAdd10Times_ThenGet10()
        {
            var totalObject = new Total();

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => totalObject.IncrementTotal()));
            }
            await Task.WhenAll(tasks);

            Assert.Equal(10, totalObject.TotalValue);
        }
    }
    public class Total
    {
        private int total = 0;
        private static readonly object syncHandle = new
        {
            Index = 1
        };
        // private static readonly int syncHandle = 1;

        public int TotalValue
        {
            get
            {
                lock (syncHandle) { return total; }
            }
        }
        public void IncrementTotal()
        {
            lock (syncHandle) { total++; }
        }
    }

    public class TotalWithMonitor
    {
        private int total = 0;
        private static readonly object syncHandle = new
        {
            Index = 1
        };
        // private static readonly int syncHandle = 1;

        public int TotalValue
        {
            get
            {
                System.Threading.Monitor.Enter(syncHandle);
                try
                {
                    return total;
                }
                finally
                {
                    System.Threading.Monitor.Exit(syncHandle);
                }
            }
        }

        public void IncrementTotal()
        {
            System.Threading.Monitor.Enter(syncHandle);
            try
            {
                total++;
            }
            finally
            {
                System.Threading.Monitor.Exit(syncHandle);
            }
        }
    }

    public sealed class LockHolder<T> : IDisposable where T : class
    {
        private T handle;
        private bool holdsLock;
        public LockHolder(T handle, int milliSecondTimeout)
        {
            this.handle = handle;
            holdsLock = System.Threading.Monitor.TryEnter(
            handle, milliSecondTimeout);
        }
        public bool LockSuccessful
        {
            get { return holdsLock; }
        }
        public void Dispose()
        {
            if (holdsLock)
                System.Threading.Monitor.Exit(handle);
            // Don't unlock twice
            holdsLock = false;
        }
    }
}