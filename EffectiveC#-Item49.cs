using System;
using Xunit;

namespace csharp_learning
{
    public class UnitTest49
    {
        [Fact]
        public void WhenFilterForCatchClause()
        {
            int number = 100;
            bool didCatchRun = false;
            try
            {
                throw new InvalidCastException();
            }
            catch (InvalidCastException ex) when (number < 200)
            {
                didCatchRun = true;                
            }

            Assert.True(didCatchRun);
        }
    }
}