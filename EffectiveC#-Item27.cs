using System.Runtime.CompilerServices;
using Xunit;

namespace csharp_learning
{
    public interface IFoo
    {
        int Marker { get; set; }
    }

    public static class FooExtentions // 1:static class 
    {
        public static void NextMarker(this IFoo thing) // 2:static method ,3: use this before type
        {
            thing.Marker += 1;
        }
    }

    public class Test
    {
        private static void UpdateMarker(MyType foo)
        {
            foo.NextMarker();
        }

        private class MyType : IFoo
        {
            public int Marker { get; set; }
            public void NextMarker() => Marker += 5;
        }

        [Fact]
        public void TestExtentionMethod()
        {
            MyType t = new MyType();
            UpdateMarker(t);
            Assert.Equal(5, t.Marker);
        }

        // 应该保证扩展方法的行为与类里面的同名方法保持一致，如果在类中以更为高效的算法重新实现早前定义的扩展方法，那么应该保持行为与它一致
    }
}