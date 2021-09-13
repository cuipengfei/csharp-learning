using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    // 如果不需要把类型参数所表示的对象设为实例字段，那么应该优先考虑创建泛型方法，而不是泛型类。
    
    public class UnitTest25
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest25(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private const double D1 = 4;
        private const double D2 = 5;

        private const string S1 = "Tom";
        private const string S2 = "Ben";
        
        [Fact]
        public void OldUtilsCompare()
        {
            Assert.Equal(5, OldUtils<double>.Max(D1, D2));
            Assert.Equal(4, OldUtils<double>.Min(D1, D2));
            
            Assert.Equal("Tom",OldUtils<string>.Max(S1,S2));
            Assert.Equal("Ben",OldUtils<string>.Min(S1,S2));
        }

        [Fact]
        public void NewUtilsCompare()
        {
            Assert.Equal(5, NewUtils.Max(D1, D2));
            Assert.Equal(4, NewUtils.Min(D1, D2));
            
            Assert.Equal("Tom",NewUtils.Max(S1,S2));
            Assert.Equal("Ben",NewUtils.Min(S1,S2));
        }

        [Fact]
        public void CommaSeparatedList()
        {
            var strings = new List<string>() {"First", "Second", "Third", "Fourth"};
            var ints = new List<int>() {1, 2, 3, 4};

            CommaSeperatedListBuilder builder = new CommaSeperatedListBuilder();
            CommaSeperatedListBuilder.Add(strings);
            CommaSeperatedListBuilder.Add(ints);
            var result = builder.ToString();
            _testOutputHelper.WriteLine(result);
        }
    }
    
    public static class OldUtils<T>
    {
        public static T Max(T left, T right) => Comparer<T>.Default.Compare(left, right) >= 0 ? left : right;
        public static T Min(T left, T right) => Comparer<T>.Default.Compare(left, right) <= 0 ? left : right;
    }

    public static class NewUtils
    {
        public static T Max<T>(T left, T right) => Comparer<T>.Default.Compare(left, right) >= 0 ? left : right;
        public static T Min<T>(T left, T right) => Comparer<T>.Default.Compare(left, right) <= 0 ? left : right;

        public static double Max(double left, double right) => Math.Max(left, right);
        public static double Min(double left, double right) => Math.Min(left, right);
    }

    public class CommaSeperatedListBuilder
    {
        private static readonly StringBuilder Storage = new StringBuilder();

        public static void Add<T>(IEnumerable<T> items)
        {
            foreach (var item in items) {
                if (Storage.Length > 0) Storage.Append(", ");
                Storage.Append('"');
                Storage.Append(item.ToString());
                Storage.Append('"');
            }
        }

        public override string ToString() => Storage.ToString();
    }
}