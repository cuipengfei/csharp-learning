using System;
using System.Linq.Expressions;
using Xunit;

#pragma warning disable CA1305
// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable PossibleNullReferenceException
// ReSharper disable UnusedType.Local
// ReSharper disable UnusedMember.Local

namespace csharp_learning
{
    public class MoreEffectiveCSharpItem43
    {
        // 43. 了解动态编程的优点及缺点

        // 自由变换类型
        
        [Fact]
        private void DynamicType()
        {
            dynamic dy;

            dy = 1;
            Assert.IsType<int>(dy);

            dy = 1.1;
            Assert.IsType<double>(dy);

            dy = 1.1m;
            Assert.IsType<decimal>(dy);

            dy = "hello";
            Assert.IsType<string>(dy);
        }

        // 运算结果也可以跟随变换类型
        
        [Fact]
        private void DynamicAdd()
        {
            dynamic left, right, result;

            left = 1;
            right = 2;
            result = Add(left, right);
            Assert.IsType<int>(result);
            Assert.Equal(3, result);

            left = "Thought";
            right = "works";
            result = Add(left, right);
            Assert.IsType<string>(result);
            Assert.Equal("Thoughtworks", result);

            left = new DateTime(2021, 12, 10);
            right = new TimeSpan(1, 0, 0, 0);
            result = Add(left, right);
            Assert.IsType<DateTime>(result);
            Assert.Equal(new DateTime(2021, 12, 11), result);
        }

        private static dynamic Add(dynamic left, dynamic right)
        {
            return left + right;
        }

        // 从 dynamic 变为强类型
        
        [Fact]
        private void DynamicToSpecificType()
        {
            dynamic dy = 1;
            Assert.IsType<int>(dy);

            var i = Convert.ToDouble(dy);
            Assert.IsType<double>(i);
        }

        // 如果编码时不能确定数据类型但一定要在运行时调用对象的某个方法，则可以传入委托定义这个方法

        private static TResult DynamicAdd2<T1, T2, TResult>(T1 left, T2 right, Func<T1, T2, TResult> addMethod)
        {
            return addMethod(left, right);
        }

        [Fact]
        private void LambdaDynamicAdd1()
        {
            var left1 = 1;
            var right1 = 2;
            var lambdaResult1 = DynamicAdd2(left1, right1, (a, b) => a + b);
            Assert.Equal(3,lambdaResult1);

            var left2 = new DateTime(2021, 12, 10);
            var right2 = new TimeSpan(1, 0, 0, 0);
            var lambdaResult2 = DynamicAdd2(left2, right2, (a, b) => a.Add(b));
            Assert.Equal(new DateTime(2021,12,11), lambdaResult2);
        }
        
        // 但这样每次都要我们自己传入一个委托，调用越多重复代码则可能越多

        private static T AddExpression<T>(T left, T right)
        {
            var leftOperand = Expression.Parameter(typeof(T), "left");
            var rightOperand = Expression.Parameter(typeof(T), "right");
            var body = Expression.Add(leftOperand, rightOperand);

            var adder = Expression.Lambda<Func<T, T, T>>(body, leftOperand, rightOperand);
            var @delegate = adder.Compile();

            return @delegate(left, right);
        }

        [Fact]
        private void LambdaDynamicAdd2()
        {
            dynamic left = 1;
            dynamic right = 2;
            var result = AddExpression(left, right);
            Assert.IsType<int>(result);
            Assert.Equal(3, result);
        }
        
        // 处理不同的类型运算

        private static TResult AddExpression<T1, T2, TResult>(T1 left, T2 right)
        {
            var leftOperand = Expression.Parameter(typeof(T1), "left");
            var rightOperand = Expression.Parameter(typeof(T2), "right");
            var body = Expression.Add(leftOperand, rightOperand);

            var adder = Expression.Lambda<Func<T1, T2, TResult>>(body, leftOperand, rightOperand);
            var @delegate = adder.Compile();

            return @delegate(left, right);
        }

        [Fact]
        private void LambdaDynamicAdd3()
        {
            dynamic left = new DateTime(2021, 12, 10);
            dynamic right = new TimeSpan(1, 0, 0,0);
            var result = AddExpression<DateTime, TimeSpan, DateTime>(left, right);
            Assert.IsType<DateTime>(result);
            Assert.Equal(new DateTime(2021, 12, 11), result);
        }
        
        // 但是编译表达式比较费事，所以可以把编译好的 delegate 缓存起来

        private static class BinaryOperator<T1, T2, TResult>
        {
            private static Func<T1, T2, TResult> _compliedExpression;

            public static TResult Add(T1 left, T2 right)
            {
                if (_compliedExpression == null)
                {
                    CreateFunc();
                }

                return _compliedExpression(left, right);
            }

            private static void CreateFunc()
            {
                var leftOperand = Expression.Parameter(typeof(T1), "left");
                var rightOperand = Expression.Parameter(typeof(T2), "right");
                var body = Expression.Add(leftOperand, rightOperand);

                var adder = Expression.Lambda<Func<T1, T2, TResult>>(body, leftOperand, rightOperand);
                _compliedExpression = adder.Compile();
            }
        }
    }
}