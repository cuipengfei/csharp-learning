using System.Collections.Generic;
using FluentAssertions;
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
            var resultFromLoop = BuildTupleWithLoop();
            var resultFromQueryStatement = BuildTupleWithQueryStatement();

            resultFromLoop.Should().Equal(resultFromQueryStatement);
        }

        private static IEnumerable<Tuple<int, int>> BuildTupleWithQueryStatement()
        {
            return from x in Enumerable.Range(0, 10)
                from y in Enumerable.Range(0, 10)
                where x + y < 10
                select Tuple.Create(x, y);
        }

        private static IEnumerable<Tuple<int, int>> BuildTupleWithLoop()
        {
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    if (x + y < 10)
                    {
                        yield return Tuple.Create(x, y);    
                    }
                }
            }
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