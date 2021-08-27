using System;
using Xunit;

namespace csharp_learning
{
    public class EffictiveC__Item3
    {
        private Object o1 = Factory.getObject();
        private Object o2 = Factory.getObject();
        private Object o3 = Factory.GetErrorClass();

        public void maybeThrow()
        {
            ErrorClass errorClass = (ErrorClass) o2;
        }
        
        public string maybeSuccess()
        {
            ErrorClass errorClass = o3 as ErrorClass;
            try
            {
                MyClass t;
                t = (MyClass) errorClass;
                // t = errorClass as MyClass;
                return "ok";
            }
            catch
            {
                return "error";
            }
        }
        
        public void cant()
        {
            // int num = o1 as int;
        }

        [Fact]
        public void ShouldNotNullIfRight()
        {
            MyClass myClass1 = o1 as MyClass;
            MyClass myClass2 = (MyClass) o2;
            Assert.NotNull(myClass1);
            Assert.NotNull(myClass2);
        }

        [Fact]
        public void ShouldThrowIfWrong()
        {
            ErrorClass myClass1 = o1 as ErrorClass;
            
            Assert.Null(myClass1);
            Assert.Throws<InvalidCastException>(maybeThrow);
        }
        
        [Fact]
        public void ShouldSuccess()
        {
            Assert.Equal(maybeSuccess(), "ok");
        }
    }

    class MyClass
    {
    }

    class ErrorClass
    {
        // fail whether have or not
        
        private MyClass _value;
        
        public static implicit operator MyClass(ErrorClass e)
        {
            return e._value;
        }
    }

    static class Factory
    {
        public static Object getObject()
        {
            return new MyClass();
        }

        public static ErrorClass GetErrorClass()
        {
            return new ErrorClass();
        }
    }
    
}