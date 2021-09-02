using System;
using System.Diagnostics;
using Xunit;

namespace csharp_learning
{
    // 合理利用异常筛选器的副作用来实现某些效果
    // 打印未捕获异常的日志或打印所有异常的日志；
    public class UnitClass50
    {
        private int _retryTimes = 0;
        
        [Fact]
        public void ShouldThrowExceptionWhenGivenANotPositiveNumber()
        {
            Action action = () => InvokePositiveAdd(0, 2);
            Exception exception = Assert.Throws<InvalidInputException>(action) ;
            Assert.Equal("number must be a positive", exception.Message);
        }
        
        [Fact]
        public void ShouldReturn10WhenGivenAIs4AndBIs6()
        {
            // InvokePositiveAdd(4, 6);
            Assert.Equal(10, InvokePositiveAdd(4, 6));
        }

        public int InvokePositiveAdd(int a, int b)
        {
            try
            {
                var result = PositiveAdd(a, b);
                return result;
            }
            catch (Exception e) when (ConsoleLogException(e))
            {

            }
            catch (TimeoutException ex) when(_retryTimes++ < 5 && (Debugger.IsAttached))
            {
                InvokePositiveAdd(a, b);
            }

            return 0;
        }

        public int PositiveAdd(int a, int b)
        {
            var random = new Random();
            var seconds = random.Next(0, 5);
            Console.WriteLine("wait {0} seconds for calculating result", seconds);
            
            if (seconds > 5)
            {
                throw new TimeoutException("Timeout!!!");
            }
            
            if (a <= 0 || b <= 0)
            {
                throw new InvalidInputException("number must be a positive");
            }
            return a + b;
        }

        public static bool ConsoleLogException(Exception e)
        {
            Console.WriteLine("Error Happened: {0}", e);
            return false;
        } 
        
        public class InvalidInputException : Exception
        {
            public InvalidInputException(string? message) : base(message)
            {
            }
        }
    }
}