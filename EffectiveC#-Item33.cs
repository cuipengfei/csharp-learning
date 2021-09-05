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
        public static int count1 = 0;
        public static int count2 = 0;

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
                collection.Add(startAt + i * i * stepBy);
                count1++;
            }

            return collection;
        }

        static IEnumerable<int> CreateSequenceOnlyAfterUsed(int numberOfElements, int startAt, int stepBy)
        {
            for (var i = 0; i < numberOfElements; i++)
            {
                count2++;
                yield return startAt + i * i * stepBy;
            }
        }

        [Fact]
        void CreatSequenceBasicTest()
        {
            count1 = 0;
            count2 = 0;
            var t1 = CreateSequenceAllAtOnce(10000, 0, 7);
            foreach (int i in t1)
            {
                if (i > 100)
                {
                    break;
                }

            }

            var t2 = CreateSequenceOnlyAfterUsed(10000, 0, 7);
            foreach (int i in t2)
            {
                if (i > 100)
                {
                    break;
                }
            }

            Assert.Equal(10000, count1);
            Assert.Equal(5, count2);
        }
        
    }
}