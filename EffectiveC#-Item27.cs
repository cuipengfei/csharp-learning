using System.Runtime.CompilerServices;
using Xunit;

namespace csharp_learning
{

    public interface IFoo
    {
        int Marker { get; set; }
    }

    public static class FooExtentions
    {
        public static void NextMarker(this IFoo thing)
        {
            thing.Marker += 1;
        }
    }

    public class Test
    {
        public static void UpdateMarker(MyType foo)
        {
            foo.NextMarker();
        }

        public class MyType : IFoo
        {
            public int Marker { get; set; }

            public void NextMarker() => Marker += 5;
        }

        [Fact]
        public void TestExtentionMethod()
        {
            MyType t = new MyType();
            UpdateMarker(t);
            Assert.Equal(5,t.Marker);
        }
    }
}