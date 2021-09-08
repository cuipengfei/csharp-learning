using Xunit;

namespace csharp_learning
{
    // 绝对不要在构造函数中调用虚函数
    public class EffectiveC__Item16
    {
        class B
        {
            public int num = 0;
            
            protected B()
            {
                VFunc();
            }

            protected virtual void VFunc()
            {
                num = 1;
            }
        }

        class Derived : B
        {
            public readonly int num2 = 2;
            
            public Derived(int num)
            {
                num2 = num;
            }

            protected override void VFunc()
            {
                num = num2;
            }
        }

        [Fact]
        public void NumTest()
        {
            var d = new Derived(4);
            Assert.Equal(2,d.num);
            Assert.Equal(4,d.num2);
        }
        
        abstract class B2
        {
            public int num = 0;
            
            protected B2()
            {
                VFunc();
            }

            protected abstract void VFunc();
        }

        class Derived2 : B2
        {
            public readonly int num2 = 2;
            
            public Derived2(int num)
            {
                num2 = num;
            }

            protected override void VFunc()
            {
                num = num2;
            }
        }
        
        [Fact]
        public void NumTest2()
        {
            var d = new Derived2(4);
            Assert.Equal(2,d.num);
            Assert.Equal(4,d.num2);
        }
    }
}