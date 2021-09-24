using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Xunit;

namespace System.Collections.Generic
{
    // 声明字段时，尽量直接为其设定初始值

    public class NoNeedInitiateProperty
    {
        private List<string> labels = new List<string>();

        NoNeedInitiateProperty()
        {
        }

        NoNeedInitiateProperty(int size)
        {
            // labels = new List<string>(); // 实际中会多余执行一次
            labels = new List<string>(size);
        }
    }

    // 除非三种情况下，建议不使用直接设置初始值
    public class Test
    {
        [Fact]
        public void TestCase1()
        {
            // 情况一：每种值类型均有一个隐式的默认构造函数，struct的默认构造函数将所有的值类型字段设置为默认值并将所有的引用类型字段设置为 null
            var myClass = new MyClass1();
            var num1 = myClass.MyStruct1.SomethingInStruct;
            var num2 = myClass.MyStruct2.SomethingInStruct;

            Assert.Equal(0, myClass.MyStruct1.SomethingInStruct);
            Assert.Equal(0, myClass.MyStruct2.SomethingInStruct);
            Assert.Null(myClass.MyStruct1.ClassInStruct);
            Assert.Null(myClass.MyStruct2.ClassInStruct);
        }

        private class MyClass1
        {
            public MyStruct MyStruct1;
            public MyStruct MyStruct2 = new MyStruct();
        }

        public struct MyStruct
        {
            public int SomethingInStruct { get; set; }
            public TestClass ClassInStruct;
        }

        public class TestClass
        {
        }

        // 情况二： 当属性需要不同行为的初始化行为时，分别在构造函数中初始化

        public class MyClass2
        {
            private List<string> labels = new List<string>();

            MyClass2()
            {
            }

            MyClass2(int size)
            {
                // labels = new List<string>(); // 实际中会多余执行一次
                labels = new List<string>(size);
            }
        }

        // 情况三：初始化时会可能会产生异常,无法在声明时处理
        public class MyClass3
        {
            public MayThrowException MayThrowException;

            MyClass3()
            {
                try
                {
                    MayThrowException = new MayThrowException();
                }
                catch
                {
                    MayThrowException = null;
                }
            }
        }
    }

    public class MayThrowException
    {
        public MayThrowException()
        {
            throw new Exception();
        }
    }
}