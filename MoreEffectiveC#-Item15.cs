using Xunit;

namespace System.Linq
{
    //理解接口方法与虚方法之间的区别
    public class MoreEffectiveCItem15
    {
        //接口方法：实现接口中的方法
        // 虚方法：在子类中重写定义来基类中的virtual 方法

        interface IMessage
        {
            string Message();
        }

        public class MyClass : IMessage
        {
            public string Message() => nameof(MyClass);
        }
        
        public class ChildClass : MyClass
        {
            // z子类没有重写父类的Message方法，而是创建了新的版本，将父类的Message()隐藏起来
            public new string Message() => nameof(ChildClass);
        }

        [Fact]
        public void ShouldOutputCorrectResult()
        {
            var myClass = new MyClass();
            var imessage = myClass as IMessage;
            var childClass = new ChildClass();
            var ichildmessage = childClass as IMessage;
            
            Assert.Equal("MyClass", myClass.Message());
            Assert.Equal("MyClass", imessage.Message());
            Assert.Equal("ChildClass", childClass.Message());
            Assert.Equal("MyClass", ichildmessage.Message());
        }
        
        
        public class ChildClassWithImplementInterface : MyClass, IMessage
        {
            public new string Message() => nameof(ChildClassWithImplementInterface);
        }
        
        
        [Fact]
        public void ShouldOutputChildClassWhenUsingInterfaceInstance()
        {
            var mChildClassWithImplementInterface = new ChildClassWithImplementInterface();
            // 现在本类的定义中寻找对接口中某个方法的实现，如果找不到才会去基类中找；
            var imessage = mChildClassWithImplementInterface as IMessage;
            // 通过基类的引用来访问基类中的方法
            var superClass = mChildClassWithImplementInterface as MyClass;

            Assert.Equal("ChildClassWithImplementInterface", mChildClassWithImplementInterface.Message());
            Assert.Equal("ChildClassWithImplementInterface", imessage.Message());
            Assert.Equal("MyClass", superClass.Message());
        }
        
        
        
        // 如果想子类只调用子类自己的方法的话；
        public class MyClassForOverride : IMessage
        {
            public virtual string Message() => nameof(MyClassForOverride);
        }
        
        public class ChildClassForOverride : MyClassForOverride
        {
            public override string Message() => nameof(ChildClassForOverride);
        }

        [Fact]
        public void ShouldOnlyReturnChildMethod()
        {
            var childClass = new ChildClassForOverride();
            var interfaceReference = childClass as IMessage;
            var superClassReference = childClass as MyClassForOverride;
            
            Assert.Equal("ChildClassForOverride", childClass.Message());
            Assert.Equal("ChildClassForOverride", interfaceReference.Message());
            Assert.Equal("ChildClassForOverride", superClassReference.Message());
        }
        
        // 如果想达到上面这种目的，但不想写virtual方法，可以通过abstract类来实现；
        public abstract class MyClassWithAbstract : IMessage
        {
            public abstract string Message();
        }
        
        public abstract class ChildClassForAbstract: MyClassWithAbstract
        {
            public override string Message() => nameof(ChildClassForAbstract);
        }
    }
}