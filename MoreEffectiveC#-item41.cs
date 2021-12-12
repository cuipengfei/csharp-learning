namespace System.Threading.Tasks
{
    public class MoreEffective__item41
    {
        // 若其他线程也可以访问到这个对象，率先加锁，这段代码可能就会一直卡在这里
        public void SampleMethod()
        {
            var lockingExample = new LockingExample();
            lockingExample.SomeMethod();
        }

    }
    
    public class LockingExample
    {
        public void SomeMethod()
        {
            lock (this)
            {
                // Some implementation
            }
        }
        
        // 添加一个私有handle, object,， 用来加锁保护共享资源
        private object synHandler;

        private object GetSynHandler()
        {
            Interlocked.CompareExchange(ref synHandler, new object(), null);
            return synHandler;
        }

        public void AnotherMethod()
        {
            lock (GetSynHandler())
            {
                // Some implementation
            }
        }
    }
    
    
}