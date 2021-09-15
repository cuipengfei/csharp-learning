using System;
using Xunit;

namespace csharp_learning
{
    // https://www.diffchecker.com/IA1sqZ60

    class MyClassA : IDisposable
    {
        public bool IsDisMethodCalled { get; private set; }

        public MyClassA()
        {
            IsDisMethodCalled = false;
        }

        public void Dispose()
        {
            // do nothing
            IsDisMethodCalled = true;
        }
    }

    public class UnitTest46
    {
        private MyClassA myClassA = new MyClassA();

        [Fact]
        public void UseDisposableResourceInUsing()
        {
            using (myClassA)
            {
                Assert.False(myClassA.IsDisMethodCalled);
            }

            Assert.True(myClassA.IsDisMethodCalled);
        }

        [Fact]
        public void UseDisposableResourceInTryCatchFinally()
        {
            try
            {
                Assert.False(myClassA.IsDisMethodCalled);
            }
            finally
            {
                if (myClassA != null)
                {
                    myClassA.Dispose();
                }
            }

            Assert.True(myClassA.IsDisMethodCalled);
        }
    }
}