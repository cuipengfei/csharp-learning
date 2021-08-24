using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_learning
{

    public class UnitTest34
    {
        [Fact]
        public void FunctionAsParameter()
        {
            var listOfNumbers = new List<int> { 1, 2, 3, 4, 5 };

            Func<int, bool> biggerThan3 = (int n) => n > 3;

            var filteredResults = listOfNumbers.Where(biggerThan3);

            Assert.Equal(new int[] { 4, 5 }, filteredResults);
        }

        [Fact]
        public void InstanceOfInterfaceAsParameter()
        {
            var listOfNumbers = new List<int> { 1, 2, 3, 4, 5 };

            IMyPredicate<int> biggerThan3 = new BiggerThan3();

            var filteredResults = listOfNumbers.MyWhere(biggerThan3);

            Assert.Equal(new int[] { 4, 5 }, filteredResults);
        }

    }

    public interface IMyPredicate<INPUT_TYPE>
    {
        bool check(INPUT_TYPE input);
    }

    class BiggerThan3 : IMyPredicate<int>
    {
        public bool check(int input)
        {
            return input > 3;
        }
    }

    public static class MyExtensionMethods
    {
        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, IMyPredicate<T> myPredicate)
        {
            foreach (var item in source)
            {
                if (myPredicate.check(item))
                {
                    yield return item;
                }
            }
        }
    }

}
