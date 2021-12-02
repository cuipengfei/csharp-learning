using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{

    public class MoreEffectiveCSharpItem30
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public MoreEffectiveCSharpItem30(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        //真的有耗时较长的工作，再拿到async里面去做，否则，切换上下文的成本有可能超过async的收益
        [Fact]
        public void CompareTimeConsumptionOfAsyncAndSync()
        {
            var timeA = Time(async () =>
                {
                    var a1 = DoA1();
                    var a2 = DoA2();
                    await Task.WhenAll(a1, a2);
                });

            var timeB = Time(() =>
               {
                   DoB1();
                   DoB2();
               });

            _testOutputHelper.WriteLine(timeA.TotalMilliseconds.ToString()); //我的某台机器2.几毫秒
            _testOutputHelper.WriteLine(timeB.TotalMilliseconds.ToString()); //我的某台机器1.几毫秒

            //此处不写assert，因为运行时间与具体机器有关，偶尔timeA会小一些
        }

        public static TimeSpan Time(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public static async Task DoA1()
        {
            await Task.Run(() =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            Console.WriteLine("A1");
                        }
                    });
        }

        public static async Task DoA2()
        {
            await Task.Run(() =>
                      {
                          for (int i = 0; i < 100; i++)
                          {
                              Console.WriteLine("A2");
                          }
                      });
        }

        public static void DoB1()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("B1");
            }

        }

        public static void DoB2()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("B2");
            }

        }
    }
}