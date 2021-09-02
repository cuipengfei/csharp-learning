using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    public class EffectiveC__Item33
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public EffectiveC__Item33(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        // 等真正用到序列中的元素时再去生成

        static IList<int> CreateSequenceAllAtOnce(int numberOfElements, int startAt, int stepBy)
        {
            var collection = new List<int>(numberOfElements);
            for (var i = 0; i < numberOfElements; i++)
            {
                collection.Add(startAt + i *i* stepBy);
            }

            return collection;
        }

        static IEnumerable<int> CreateSequenceOnlyAfterUsed(int numberOfElements, int startAt, int stepBy)
        {
            var collection = new List<int>(numberOfElements);
            for (var i = 0; i < numberOfElements; i++)
            {
                yield return startAt + i * i * stepBy;
            }
        }

        [Fact]
        void CreatSequenceBasicTest()
        {
            var t1= CreateSequenceAllAtOnce(10000, 0, 7);
           foreach (int i  in t1)
           {
               _testOutputHelper.WriteLine("{0} ", i);

           }
           
           var t2= CreateSequenceOnlyAfterUsed(10000, 0, 7);
           foreach (int i  in t2)
           {
               _testOutputHelper.WriteLine("{0} ", i);

           }
        }

        [Fact]
        public void CreatSequenceTest()
        {
           
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var sequence1 = CreateSequenceOnlyAfterUsed(100000000, 0, 7).TakeWhile((num) => num < 1000);
            stopwatch.Stop();
            var s1 = stopwatch.ElapsedTicks;
            
            stopwatch.Start();
            var sequence2 = CreateSequenceAllAtOnce(100000000, 0, 7).TakeWhile((num) => num < 1000);
            stopwatch.Stop();
            var s2 = stopwatch.ElapsedTicks;
            
            Assert.True(s1 * 100 < s2);
        }
        
    }
}