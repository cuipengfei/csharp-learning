using System;
using System.Runtime.CompilerServices;

namespace csharp_learning
{
    public class MoreEffectiveCSharpItem21
    {
        // 17. 不要把事件声明成 virtual

        private string YouCanNewAPropertyLikeThis { get; set; }
        
        private string _field; // backing field
        private string YouCanNewAPropertyLikeThisToo
        {
            get => "I am " + _field + ".";
            set => _field = value;
        }

        private event EventHandler Event0;
        
        private EventHandler _event; // backing field
        private event EventHandler Event1
        {
            add => _event += value;
            remove => _event -= value;
        }
    }

    public class WorkerEngine
    {
        public event EventHandler<WorkerEventArgs> OnProgress;

        public void DoLotsOfStuff()
        {
            for (var i = 0; i < 100; i++)
            {
                SomeWork();
                var args = new WorkerEventArgs
                {
                    Percent = 1
                };
                OnProgress?.Invoke(this, args);
                if (args.Cancel) return;
            }
        }

        private void SomeWork()
        {
            // do something...
        }
    }

    public abstract class WorkerEngineBase
    {
        public virtual event EventHandler<WorkerEventArgs> OnProgress;

        public void DoLotsOfStuff()
        {
            for (var i = 0; i < 100; i++)
            {
                SomeWork();
                var args = new WorkerEventArgs
                {
                    Percent = i
                };
                OnProgress?.Invoke(this, args);
                if (args.Cancel) return;
            }

        }
        public event EventHandler<WorkerEventArgs> Event;

        protected abstract void SomeWork();
    }

    public class WorkerEngineDerived : WorkerEngineBase
    {
        protected override void SomeWork()
        {
            // do something...
        }

        // 以下这种写法隐藏了基类里的私有 event 字段。
        // public override event EventHandler<WorkerEventArgs> OnProgress;

        // 1. 所以需要显式指定子类的 OnProgress 访问基类的 event。就像这样：
        public override event EventHandler<WorkerEventArgs> OnProgress
        {
            add => base.OnProgress += value;
            remove => base.OnProgress -= value;
        }
    }

    public class WorkerEventArgs
    {
        public int Percent { get; set; }
        
        public bool Cancel { get; set; }
    }
    
    #region 2. 如果基类的 event 也是通过定制 add 和 remove 的办法写的，那么需要让子类也能访问并修改这个基类用来存放 handler 的字段。protected

    public abstract class WorkerEngineBase2
    {
        protected EventHandler<WorkerEventArgs> progressEvent;
        
        public virtual event EventHandler<WorkerEventArgs> OnProgress
        {
            add => progressEvent += value;
            remove => progressEvent -= value;
        }

        public void DoLotsOfStuff()
        {
            for (var i = 0; i < 100; i++)
            {
                SomeWork();
                var args = new WorkerEventArgs
                {
                    Percent = 1
                };
                progressEvent?.Invoke(this, args);

                if (args.Cancel) return;
            }
        }

        protected abstract void SomeWork();
    }

    public class WorkerEngineDerived2 : WorkerEngineBase2
    {
        protected override void SomeWork()
        {
            // do something...
        }

        public override event EventHandler<WorkerEventArgs> OnProgress
        {
            add => progressEvent += value;
            remove => progressEvent -= value;
        }
    }
    
    #endregion
    
    #region 3. 在基类中定义触发事件的 virtual 方法，并要求所有子类都重写基类的 virtual 事件和这个 virtual 方法

    public abstract class WorkerEngineBase3
    {
        public virtual event EventHandler<WorkerEventArgs> OnProgress;

        protected virtual WorkerEventArgs RaiseEvent(WorkerEventArgs args)
        {
            OnProgress?.Invoke(this, args);
            return args;
        }

        public void DoLotsOfStuff()
        {
            for (int i = 0; i < 100; i++)
            {
                SomeWork();
                var args = new WorkerEventArgs
                {
                    Percent = 1
                };
                RaiseEvent(args);
                if(args.Cancel) return;
            }
        }

        protected abstract void SomeWork();
    }

    public class WorkerEngineDerived3 : WorkerEngineBase3
    {
        protected override void SomeWork()
        {
            // do something...
        }

        public override event EventHandler<WorkerEventArgs> OnProgress;

        protected override WorkerEventArgs RaiseEvent(WorkerEventArgs args)
        {
            OnProgress?.Invoke(this, args);
            return args;
        }
    }
    
    #endregion
}