using Xunit;

namespace csharp_learning
{
    // 确保0可以当成值类型的有效状态使用
    public class MoreEffectiveC__Item05
    {
        [Fact]
        public void DefaultZero()
        {
            Planet any = new Planet();
            Assert.Equal("0", any.ToString());

            Planet another = default(Planet);
            Assert.Equal("0", another.ToString());

            Planet earth = Planet.Earth;
            Assert.Equal("Earth", earth.ToString());
        }
        
    }

    public enum Planet
    {
        // None = 0,
        Mercury = 1,
        Venus = 2,
        Earth = 3
    }

    public struct OB
    {
        public Planet whichOne;
        public double magnitude;
    }

    public struct OB2
    {
        private Planet planet;
        public double magnitude;

        public Planet PlanetOutside
        {
            get => planet == default ? Planet.Earth : planet;
            set => planet = value;
        }
    }
}