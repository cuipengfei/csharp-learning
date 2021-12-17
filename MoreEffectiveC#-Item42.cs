using System;
using System.Threading;

namespace csharp_learning
{
    // 42. 不要在加了锁的区域内调用未知的方法
    
    public class MoreEffectiveCSharpItem42
    {

        private void Engine_RaiseProgress(object sender, EventArgs e)
        {
            if (sender is BackendWorker engine)
            {
                Console.WriteLine(engine.ProgressCounter);
            }
        }
    }

    public class BackendWorker
    {
        public event EventHandler<EventArgs> RaiseProgress;
        private readonly object _syncHandle = new();
        private int _progressCounter;

        public void DoWork()
        {
            for (var count = 0; count < 100; count++)
            {
                lock (_syncHandle)
                {
                    Thread.Sleep(100);
                    _progressCounter++;
                    RaiseProgress?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public int ProgressCounter
        {
            get
            {
                lock(_syncHandle)
                {
                    return _progressCounter;
                }
            }
        }
    }
}