using System;
using System.IO;
using System.Threading;
using Xunit;

namespace csharp_learning
{
    // 如果你在泛型类里面根据类型参数创建了实例，那么就应该判断该实例所属的类型是否实现了IDisposable接口。
    // 如果实现了，就必须编写相关的代码，以防程序在离开泛型类之后发生资源泄漏
    public class TestItem21
    {
        private static bool _called;

        private interface IEngine
        {
            void DoWork();
        }

        private class EngineDriver<T> where T : IEngine, new()
        {
            public void GetThingsDoneMayCauseMemoryLeak()
            {
                var driver = new T();
                driver.DoWork();
            }

            public void GetThingsDoneCouldRecycleMemory()
            {
                //编译器会把driver视为IDisposable，并创建隐藏的局部变量，用以保存指向这个IDisposable的引用。
                //在T没有实现IDisposable的情况下，这个局部变量的值是null，此时编译器不调用Dispose（），因为它在调用之前会先做检查。
                //反之，如果T实现了IDisposable，那么编译器会生成相应的代码，以便在程序退出using块的时候调用Dispose（）方法。
                var driver = new T();
                using (driver as IDisposable)
                {
                    driver.DoWork();
                }

                // 上面的写法等同于
                /*
                var a = driver as IDisposable;
                driver.DoWork();
                a?.Dispose();
                */
            }
        }

        private sealed class EngineDriver2<T> : IDisposable where T : IEngine, new()
        {
            private Lazy<T> driver = new Lazy<T>(() => new T());

            public void GetThingsDoneCouldRecycleMemory2()
            {
                driver.Value.DoWork();
            }

            public void Dispose()
            {
                if (driver.IsValueCreated)
                {
                    var resource = driver.Value as IDisposable;
                    resource?.Dispose();
                }
            }
        }

        private class NotManagedResource : IEngine, IDisposable
        {
            private FileStream _leak;

            public void DoWork()
            {
                _leak = new FileStream(@"\fake", FileMode.Create, FileAccess.Write);
            }

            public void Dispose()
            {
                _leak.Dispose();
                _called = true;
            }
        }
        
        [Fact]
        public void Test()
        {
            var test = new EngineDriver<NotManagedResource>();
            _called = false;
            test.GetThingsDoneMayCauseMemoryLeak();
            Assert.False(_called);

            _called = false;
            test.GetThingsDoneCouldRecycleMemory();
            Assert.True(_called);
            
            var test2 = new EngineDriver2<NotManagedResource>();
            _called = false;
           
            using (test2 as IDisposable)
            {
                test2.GetThingsDoneCouldRecycleMemory2();
            }
            Assert.True(_called);
        }
    }
}