using Xunit;

namespace csharp_learning
{
    public class UniTest10
    {
        private class Base
        {
            public string Method1()
            {
                return "Base.method1";
            }

            //虚函数能够被重载，能够多态
            public virtual string Method2()
            {
                return "Base.method2";
            }
        }

        private class MyClass : Base
        {
            //调用这个函数名的时候，查看当前的编译类型是什么，根据这个编译类型调用编译类型的方法。
            public new string Method1()
            {
                return "MyClass.method1";
            }

            //调用这个函数名的时候，查看当前的运行时类型是什么，根据这个对象的实际类型调用它的方法。
            public override string Method2()
            {
                return "MyClass.method2";
            }
        }

        [Fact]
        public void Test()
        {
            MyClass myClass = new MyClass();
            Base baseClass = myClass;
            Assert.Equal("Base.method1", baseClass.Method1());
            Assert.Equal("MyClass.method2", baseClass.Method2());
            Assert.Equal("MyClass.method1", myClass.Method1());
            Assert.Equal("MyClass.method2", myClass.Method2());
        }
    }
}