using System;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace csharp_learning
{
    // 匿名类和元组
    public class MoreEffectiveCSharpItem07
    {
        // 匿名类，基本等同于一个internal, immutable, sealed class
        [Fact]
        public void AnonymousTypeExample()
        {
            var pointA = new {X = 1.0, Y = 2.0};
            var pointB = new {X = 3.0, Y = 4.0};
            var pointC = new {X = 1.0, y = 2.0}; // 属性名不相同
            var pointD = new {X = 1, Y = 2}; // 属性类型不同
            var pointE = new {Y = 2.0, X = 1.0}; // 顺序不同
            var pointInSameAssembly = MyUtil.GeneratePoint();

            // 根据内容精准识别类型是否相同,并重用类型
            Assert.Equal(pointA.GetType(), pointB.GetType());
            Assert.NotEqual(pointA.GetType(), pointC.GetType());
            Assert.NotEqual(pointA.GetType(), pointD.GetType());
            Assert.NotEqual(pointA.GetType(), pointE.GetType());
            Assert.Equal(pointA.GetType(), pointInSameAssembly.GetType());

            // 匿名类是sealed
            Assert.True(pointA.GetType().IsSealed);

            // 匿名类是不可变的，下面语句会报错
            // pointA.X = 1.2;
        }

        internal sealed class AnonymousType
        {
            public double X { get; }
            public double Y { get; }

            public AnonymousType(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        // 匿名类最显著的缺点就是没有名字，所以不能作为方法的入参和返回值
        // 但有时可以与泛型、匿名委托等结合使用在一个方法中
        public static T Transform<T>(T element, Func<T, T> transformFunc)
        {
            return transformFunc(element);
        }

        [Fact]
        public void Example()
        {
            var point = new {X = 1, Y = 2};
            var newPoint = Transform(point,
                p => new {X = p.X * 2, Y = p.Y * 2});
            newPoint.Should().BeEquivalentTo(new {X = 2, Y = 4});
        }

        
        // tuple
        [Fact]
        public void TupleIsGenericTypes()
        {
            var intTuple = new ValueTuple<int, int>();
            var pointA = (X:1, Y:2);
            var pointB = (X:1.0, Y:2.0);
            var pointC = (A:1.0, B:2.0);
            pointA.Should().BeOfType(intTuple.GetType());
            pointB.Should().BeOfType(pointC.GetType());
        }

        [Fact]
        public void TupleIsMutable()
        {
            var point = (X:1, Y:2);
            point.X = 3;
            (int Length, int Width) rectangle = point;
            
            Assert.Equal(3, point.X);
            Assert.Equal(3, rectangle.Length);
            Assert.Equal(2, rectangle.Width);
        }
    }
    // 相同点： 都是可以通过实例化语句直接定义的轻量级类型，使用起来十分方便
    // 不同点： 匿名类是不可变的，元组是可变。
    // 因为不可变性，匿名类更适合作为集合运算转换的中间变量。
    // 因为遵循结构类型，元组更适合作为方法的入参和返回值
}