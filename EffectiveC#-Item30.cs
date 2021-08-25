using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace System.Linq
{
    // 优先考虑通过查询语句，而不要使用循环语句
    public class UnitTest30
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest30(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CompareQueryStatementAndSimpleForLoop()
        {
            // for loop
            var foo = new int[100];
            for (var num = 0; num < foo.Length; num++)
            {
                foo[num] = num * num;
            }

            foreach (var i in foo)
            {
                _testOutputHelper.WriteLine(i.ToString());
            }
            
            // query statement
            var fooList = (from n in Enumerable.Range(0, 100)
                select n * n).ToArray();
            
            fooList.ForAll((n) => _testOutputHelper.WriteLine(n.ToString().ToString()));
        }


        [Fact]
        public void CompareQueryStatementAndNestedForLoop()
        {
            
        }
    }


    public static class Extensions
    {
        public static void ForAll<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
            {
                action(item);
            }
        }
    }
}