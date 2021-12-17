using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace csharp_learning
{
    // 匿名类和元组的使用
    public class MoreEffectiveCSharpItem08
    {
        // 元组作为返回值，需要为其中的每个字段赋予有意义的名字
        static (T target, int index) FindFirst<T>(IEnumerable<T> enumerable, T value)
        {
            int index = 0;
            foreach (T element in enumerable)
            {
                if (element.Equals(value))
                {
                    return (value, index);
                }

                index++;
            }

            return (default(T), -1);
        }

        [Fact]
        public void AssignToATupleOrDifferentVariables()
        {
            var list = new List<string> {"a", "b", "c"};

            var result = FindFirst(list, "b");
            Assert.Equal("b", result.target);
            Assert.Equal(1, result.index);

            var (target, index) = FindFirst(list, "b");
            Assert.Equal("b", target);
            Assert.Equal(1, index);
        }

        // 匿名类如果作为入参或返回值，需要使用泛型方法
        static IEnumerable<T> FindValue<T>(IEnumerable<T> enumerable, T value)
        {
            foreach (T element in enumerable)
            {
                if (element.Equals(value))
                {
                    yield return element;
                }
            }
        }

        [Fact]
        public void UseGenericMethod()
        {
            IDictionary<int, string> numberDescriptionDictionary = new Dictionary<int, string>()
            {
                {1, "one"}, {2, "two"}, {3, "three"}, {4, "four"}, {5, "five"}, {6, "six"}, {7, "seven"}, {8, "eight"},
                {9, "nine"}, {10, "ten"},
            };
            List<int> numbers = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            // 构造一个匿名类的集合
            var list = from n in numbers
                where n % 2 == 0
                select new
                {
                    Number = n,
                    Description = numberDescriptionDictionary[n]
                };

            var res = from n in FindValue(list, new {Number = 2, Description = "two"})
                select n;
            Assert.Equal(new {Number = 2, Description = "two"}, res.First());
        }
        
        // 如果要做一些更复杂的操作，需要使用高阶函数，将方法作为入参或返回值
        public void Test()
        {
            IDictionary<int, string> numberDescriptionDictionary = new Dictionary<int, string>()
            {
                {1, "one"}, {2, "two"}, {3, "three"}, {4, "four"}, {5, "five"}, {6, "six"}, {7, "seven"}, {8, "eight"},
                {9, "nine"}, {10, "ten"},
            };
            List<int> numbers = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var list = from n in numbers
                where n % 2 == 0
                select new
                {
                    Number = n,
                    Description = numberDescriptionDictionary[n]
                };
            
            // 将针对匿名类操作的predicate函数作为入参
            var res = list.TakeWhile(i => i.Number > 5);
        }
    }
}