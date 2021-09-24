using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    // 24. 如果有泛型方法，就不要再创建针对基类或接口的重载。
    
    public class MyBase
    {
        
    }

    public interface IMessageWriter
    {
        string WriteMessage();
    }

    public class MyDerived : MyBase, IMessageWriter
    {
        string IMessageWriter.WriteMessage() => "Inside MyDerived.WriteMessage";
    }

    public class AnotherType : IMessageWriter
    {
        public string WriteMessage() => "Inside AnotherType.WriteMessage";
    }

    public class UnitTest24
    {
        private string WriteMessage(MyBase b)
        {
            // _testOutputHelper.WriteLine("Inside WriteMessage(MyBase). MyBase is " + b.GetType());
            return ("Inside WriteMessage(MyBase). MyBase is " + b.GetType());
        }

        private string WriteMessage<T>(T obj)
        {
            // _testOutputHelper.WriteLine("Inside WriteMessage<T>(T): ");
            // _testOutputHelper.WriteLine(obj.ToString());
            return ("Inside WriteMessage<T>(T): " + obj);
        }

        private string WriteMessage(IMessageWriter obj)
        {
            // _testOutputHelper.WriteLine("Inside WriteMessage(IMessageWriter): ");
            // obj.WriteMessage();
            return "Inside WriteMessage(IMessageWriter): " + obj.WriteMessage();
        }

        [Fact]
        private void MainTest()
        {
            var myDerived = new MyDerived();
            Assert.Equal($"Inside WriteMessage<T>(T): {nameof(csharp_learning)}.{nameof(MyDerived)}", WriteMessage(myDerived));
            Assert.Equal($"Inside WriteMessage(IMessageWriter): Inside {nameof(MyDerived)}.{nameof(WriteMessage)}", WriteMessage((IMessageWriter)myDerived));
            Assert.Equal($"Inside WriteMessage(MyBase). MyBase is {nameof(csharp_learning)}.{nameof(MyDerived)}", WriteMessage((MyBase)myDerived));
            
            var anotherType = new AnotherType();
            Assert.Equal($"Inside WriteMessage<T>(T): {nameof(csharp_learning)}.{nameof(AnotherType)}", WriteMessage(anotherType));
            Assert.Equal($"Inside WriteMessage(IMessageWriter): Inside {nameof(AnotherType)}.{nameof(WriteMessage)}", WriteMessage((IMessageWriter)anotherType));
        }
    }
}