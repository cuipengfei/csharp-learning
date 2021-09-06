using System;
using Xunit;

namespace csharp_learning
{

    class Animal { }
    class Cat : Animal { }
    class Dog : Animal { }

    public class UnitTest1
    {
        Animal CreateAnimal()
        {
            return new Dog();
        }

        [Fact]
        public void VarShouldNotLoseType()
        {
            var animal = CreateAnimal();

            Assert.Equal("Dog", animal.GetType().Name);
        }
    }
}
