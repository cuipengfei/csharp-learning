using System;
using System.Collections.Generic;
using Xunit;

namespace csharp_learning
{
    class Counter1
    {
        public int TotalCount { get; private set; }

        public void CountOdd(IEnumerable<int> data)
        {
            foreach (var num in data)
            {
                if (num == 55)
                {
                    //假装特殊情况，抛异常
                    throw new InvalidOperationException();
                }

                if (num % 2 == 1)
                {
                    TotalCount += 1;
                }
            }
        }
    }

    class Counter2
    {
        public int TotalCount { get; private set; }

        public void CountOdd(IEnumerable<int> data)
        {
            int tmpCount = 0;
            foreach (var num in data)
            {
                if (num == 55)
                {
                    //假装特殊情况，抛异常
                    throw new InvalidOperationException();
                }

                if (num % 2 == 1)
                {
                    tmpCount += 1;
                }
            }

            TotalCount += tmpCount;
        }
    }

    public class UnitTest48
    {
        [Fact]
        public void NoExceptionGuaranteeTest()
        {
            var counter1 = new Counter1();
            counter1.CountOdd(new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9});

            Assert.Equal(5, counter1.TotalCount);

            try
            {
                counter1.CountOdd(new List<int> {50, 51, 52, 53, 54, 55, 56, 57, 58, 59});
            }
            catch
            {
                //假装外层抓到后并处理了异常
            }

            //此时TotalCount处于inconsistent状态
            Assert.Equal(7, counter1.TotalCount);
        }

        [Fact]
        public void ExceptionGuaranteeTest()
        {
            var counter2 = new Counter2();
            counter2.CountOdd(new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9});

            Assert.Equal(5, counter2.TotalCount);

            try
            {
                counter2.CountOdd(new List<int> {50, 51, 52, 53, 54, 55, 56, 57, 58, 59});
            }
            catch
            {
                //假装外层抓到后并处理了异常
            }

            //此时TotalCount处于仿佛第二次count odd从未发生的状态
            Assert.Equal(5, counter2.TotalCount);
        }
    }
}