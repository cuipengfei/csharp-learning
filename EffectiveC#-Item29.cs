using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_learning
{
    // 优先考虑提供迭代器方法，而不要返回集合
    public class EffectiveC__Item29
    {
        [Fact]
        public void ShouldNotThrow()
        {
            Assert.False(ThrowOrNot()); // 参数判断在实际调用之前不会执行
        }

        [Fact]
        public void ShouldThrow()
        {
            Assert.True(ThrowWhenActuallyUse()); // 参数判断在实际调用时执行
            Assert.True(ThrowWhenCall()); // 参数判断在创建时就执行
        }

        public bool ThrowOrNot()
        {
            try
            {
                var collections = Item29.GenerateAlphabetSubset('g', 'b');
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

        public bool ThrowWhenActuallyUse()
        {
            try
            {
                var collections = Item29.GenerateAlphabetSubset('g', 'b');
                var array = collections.ToArray();
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

        public bool ThrowWhenCall()
        {
            try
            {
                var collections = Item29.GenerateAlphabetSubsetOne('g', 'b');
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }

    public static class Item29
    {
        // 生成小写序列 Func
        public static IEnumerable<char> GenerateAlphabet()
        {
            var letter = 'a';
            while (letter <= 'z')
            {
                yield return letter;
                letter++;
            }
        }
        // 编译器会把上面的方法生成类似迭代器的类 Current MoveNext

        // 生成两个参数之间的序列
        public static IEnumerable<char> GenerateAlphabetSubset(char first, char last)
        {
            if (last < first)
            {
                throw new ArgumentException("last must be at least as large as first");
            }

            var letter = first;
            while (letter <= last)
            {
                yield return letter;
                letter++;
            }
        }

        // 为了使得参数检查在一开始就进行，可以拆分成两个方法
        public static IEnumerable<char> GenerateAlphabetSubsetOne(char first, char last)
        {
            if (last < first)
            {
                throw new ArgumentException("last must be at least as large as first");
            }

            return GenerateAlphabetSubsetTwo(first, last);
        }

        private static IEnumerable<char> GenerateAlphabetSubsetTwo(char first, char last)
        {
            var letter = first;
            while (letter <= last)
            {
                yield return letter;
                letter++;
            }
        }
    }
}