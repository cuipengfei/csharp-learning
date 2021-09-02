using System;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace csharp_learning
{
    public class UnitTest43
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest43(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void SingleLoopsAllItems()
        {
            bool firstRun = false;
            bool secondRun = false;

            Func<int>[] funcs = new Func<int>[]
            {
                () => {firstRun=true;return 3;},
                () => {secondRun=true;return 6;}
            };

            int found = funcs
            .Select(f => f())
            .Single(n => n < 5);

            Assert.True(firstRun);
            Assert.True(secondRun);
        }

        [Fact]
        public void FirstLoopsUntilFind()
        {
            bool firstRun = false;
            bool secondRun = false;

            Func<int>[] funcs = new Func<int>[]
            {
                () => {firstRun=true;return 3;},
                () => {secondRun=true;return 6;}
            };

            int found = funcs
            .Select(f => f())
            .First(n => n < 5);

            Assert.True(firstRun);
            Assert.False(secondRun);
        }

    }
}
