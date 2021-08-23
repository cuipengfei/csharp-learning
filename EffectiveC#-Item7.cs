using System;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace csharp_learning
{


    public class UnitTest7
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest7(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ChainedDelegatesOnlyKeepsReturnedValueOfLastOne()
        {
            bool didTheFirstOneRun = false;

            Func<int, int> addOne = (int input) =>
            {
                _testOutputHelper.WriteLine("going to add 1");
                didTheFirstOneRun = true;
                return input + 1;
            };
            Func<int, int> addTwo = (int input) =>
            {
                _testOutputHelper.WriteLine("going to add 2");
                return input + 2;
            };

            Func<int, int> chain = addOne + addTwo;

            int result = chain(5);

            Assert.Equal(7, result);
            Assert.Equal(true, didTheFirstOneRun);
        }

        [Fact]
        public void ResponsibilityChainTest()
        {


            Func<int, int> addOne = (int input) =>
            {
                _testOutputHelper.WriteLine("going to add 1");
                return input + 1;
            };
            Func<int, int> addTwo = (int input) =>
            {
                _testOutputHelper.WriteLine("going to add 2");
                return input + 2;
            };

            Func<int, int> chain = addOne + addTwo;

            var results = chain.GetInvocationList().Select(func => (int)func.DynamicInvoke(5));

            Assert.Equal(new int[] { 6, 7 }, results.ToArray());
        }
    }
}
