using System;
using System.Collections.Generic;
using Xunit;

namespace csharp_learning
{
    // 用委托要求泛型参数必须提供某种方法
    public class EffectiveC__Item23
    {
        // for 1
        [Fact]
        public void ShouldGetAddedNum()
        {
            int a = 3;
            int b = 4;
            int sum = a + b;
            Assert.Equal(sum, Example.Add(a, b, (a, b) => a + b));
        }

        // for 2
        [Fact]
        public void ShouldGetPointsList()
        {
            double[] xValues = {0, 1, 2, 3, 4, 5};
            double[] yValues = {0, 1, 2, 3, 4, 5};

            List<Point> values = new List<Point>(
                Feature.Zip(xValues, yValues, (x, y) => new Point(x, y))
            );
            Assert.Equal(6, values.Count);
        }
    }

    // 要求泛型参数必须实现某个方法，可以考虑委托，而不是指定实现接口

    // 1. 基于委托创建接口契约
    public static class Example
    {
        public static T Add<T>(T left, T right,
            Func<T, T, T> AddFunc) =>
            AddFunc(left, right);
    }

    // 设备原始数据存储在两个list中，依次读取存为point list
    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // 2
    // delegate TOutput Func<T1, T2, TOutput>(T1 arg1, T2 arg2)
    public static class Feature
    {
        public static IEnumerable<TOutput> Zip<T1, T2, TOutput>(IEnumerable<T1> left, IEnumerable<T2> right,
            Func<T1, T2, TOutput> generator)
        {
            IEnumerator<T1> leftSequence = left.GetEnumerator();
            IEnumerator<T2> rightSequence = right.GetEnumerator();
            while (leftSequence.MoveNext() && rightSequence.MoveNext())
            {
                yield return generator(leftSequence.Current, rightSequence.Current);
            }

            leftSequence.Dispose();
            rightSequence.Dispose();
        }
    }
    
    // 3. 如果委托在多个方法中用到，可以放到构造函数中
}