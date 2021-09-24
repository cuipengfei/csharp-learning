using System;
using Xunit;

namespace csharp_learning
{
    // 用委托要求泛型参数必须提供某种方法
    public class EffectiveC__Item23
    {
        [Fact]
        public void ShouldGetAddedNum()
        {
            int a = 3;
            int b = 4;
            int sum = 7;
            Assert.Equal(sum, Example.Add(a, b, (a, b) => a + b));
        }
    }

    // 要求泛型参数必须实现某个方法，可以考虑委托
    
    // 基于委托创建接口契约
    public static class Example
    {
        public static T Add<T>(T left, T right, Func<T, T, T> AddFunc) => AddFunc(left, right);
    }
    
    // TODO compelete Item 23
}