using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

// 将迭代逻辑与操作（action）、谓词（predicate）及函数（function）解耦
namespace csharp_learning
{
    public class TestItem32
    {
        List<int> _listOfNumbers;
        private int count = 0;

        public static IEnumerable<T> Where<T>(IEnumerable<T> sequence, Predicate<T> filterFunc)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence), "sequence must not be null");
            if (filterFunc == null)
                throw new ArgumentNullException(nameof(sequence), "filterFunc must not be null");
            foreach (T item in sequence)
            {
                if (filterFunc(item))
                    yield return item;
            }
        }

        public static IEnumerable<T> Transform<T>(IEnumerable<T> sequence, Func<T, T> method)
        {
            foreach (T item in sequence)
            {
                yield return method(item);
            }
        }

        // 1. 无论想要执行什么操作，都可以把该操作表示成匿名的委托，并传给ForEach方法，使得集合中的每一个元素都能为这个委托所处理
        [Fact]
        public void TestActionDelegate()
        {
            _listOfNumbers = new List<int> {1, 2, 3, 4, 5};
            _listOfNumbers.ForEach(it => { count = count + it; });

            Assert.Equal(15, count);
        }

        [Fact]
        public void TestPredicateDelegate()
        {
            _listOfNumbers = new List<int> {1, 2, 3, 4, 5};
            _listOfNumbers.RemoveAll(it => it < 3);

            Assert.Equal(3, _listOfNumbers.Count);
        }

        // 2、有许多针对集合元素的复杂逻辑都可以用类似的技巧来实现

        [Fact]
        public void TestFilterDelegate()
        {
            _listOfNumbers = new List<int> {1, 2, 3, 4, 5};
            var result = new List<int>();
            foreach (int item in Where(_listOfNumbers, num => num > 3))
            {
                result.Add(item);
            }

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestTransformDelegate()
        {
            _listOfNumbers = new List<int> {1, 2, 3, 4, 5};
            var result = new List<int>();
            foreach (int item in Transform(_listOfNumbers, num => num + 1))
            {
                result.Add(item);
            }

            var expected = new List<int> {2, 3, 4, 5, 6};
            result.Should().ContainInOrder(expected);
        }
    }
}