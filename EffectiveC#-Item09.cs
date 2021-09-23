using System;
using Xunit;

namespace csharp_learning
{
    //尽量避免装箱与取消装箱这两种操作
    public class UnitTest09
    {
        [Fact]
        public void ShouldCreateNewObjectWhenBoxing()
        {
            int number = 100;

            object o = number;

            number = 200;
            
            Assert.Equal(200, number);
            Assert.Equal(100, o);
        }

        [Fact]
        public void ShouldBoxHappenedWhenUsingValueTypeInConsoleLog()
        {
            int number1 = 100;
            int number2 = 200;
            
            Console.WriteLine($"A few numbers: {number1}, {number2}");
            
        }
    }
}