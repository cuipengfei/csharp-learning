using System;
using Xunit;

namespace csharp_learning
{
    //尽量把重载方法创建的清晰、完备
    public class MoreEffectiveCItem22
    {
        public string TestOverload(float number)
        {
            return "overload from float " + number;
        }
        
        public string TestOverload(double number)
        {
            return "overload from double " + number;
        }
        
        public string TestOverload(short number)
        {
            return "overload from short " + number;
        }
        
        public string TestOverload(int number)
        {
            return "overload from int " + number;
        }
        
        
        [Fact]
        public void ShouldUseCorrectOverloadMethod()
        {
            const short factorWithShort = 1;
            Assert.Equal("overload from short 1", TestOverload(factorWithShort));

            const int factorWithInt = 1;
            Assert.Equal("overload from int 1", TestOverload(factorWithInt));
            
            const double factorWithDouble = 1.0;
            Assert.Equal("overload from double 1", TestOverload(factorWithDouble));

            const float factorWithFloat = 1;
            Assert.Equal("overload from float 1", TestOverload(factorWithFloat));
        }


        public string Overload(int x, int y)
        {
            return "overload from int and int";
        }
        
        public string Overload(int x, double y)
        {
            return "overload from int and double";
        }

        [Fact]
        public void ShouldUseCorrentOverloadWhenThereAreTwoParams()
        {
            Assert.Equal("overload from int and int", Overload(1, 1));
            Assert.Equal("overload from int and double", Overload(1, 1L));
        }


        public static string Max(double left, double right)
        {
             var maxNumber = Math.Max(left, right);
             return "max with double " + maxNumber;
        }

        public static string Max<T>(T left, T right) where T : IComparable<T>
        {
            var maxNumber = left.CompareTo(right) > 0 ? left : right;
            return "max with generic " + maxNumber;
        }


        [Fact]
        public void ShouldUsingCorrectMethodWhenGivenGenericMethod()
        {   
            Assert.Equal("max with generic 3", Max(1, 3));
            
            Assert.Equal("max with double 6", Max(1.5, 6f));
            Assert.Equal("max with generic 1.7", Max(1, 1.7f));
        }
         
    }
}