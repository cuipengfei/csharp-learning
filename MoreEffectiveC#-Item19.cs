using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace csharp_learning
{
    // 避免重载父类的方法
    public class MoreEffectiveCSharpItem19
    {
        class Fruit { }
        class Apple : Fruit { }

        class Life
        {
            public string Bar(Apple apple)
            {
                return "Life: Bar";
            }
        }
        
        class Animal: Life
        {
            public string Foo(Apple apple)
            {
                return "Animal: Foo";
            }

            public string Baz(Apple apple)
            {
                return "Animal: Baz";
            }

            public string Quz(IEnumerable<Apple> apples)
            {
                return "Animal: Quz";
            }
        }

        class Tiger: Animal
        {
            public string Foo(Fruit fruit)
            {
                return "Tiger: Foo";
            }

            public string Bar(Apple apple)
            {
                return "Tiger: Bar";
            }

            public string Baz(Apple apple1, Apple apple2 = null)
            {
                return "Tiger: Baz";
            }

            public string Quz(IEnumerable<Fruit> apples)
            {
                return "Tiger: Quz";
            }
        }

        [Fact]
        public void Test1()
        {
            var animal = new Animal();
            animal.Foo(new Apple()).Should().Be("Animal: Foo");

            var tiger = new Tiger();
            tiger.Foo(new Apple()).Should().Be("Tiger: Foo"); // 即使参数类型更加匹配，但还是从最近的派生类里找方法
            tiger.Foo(new Fruit()).Should().Be("Tiger: Foo");
        }

        [Fact]
        public void Test2()
        {
            Animal animal = new Tiger();
            animal.Foo(new Apple()).Should().Be("Animal: Foo"); // 虽然运行时类型是Tiger，但按照编译时类型往上找
            animal.Bar(new Apple()).Should().Be("Life: Bar");
            ((Tiger) animal).Foo(new Apple()).Should().Be("Tiger: Foo"); // 想使用Tiger自己的方法，还需要强转一下
            ((Tiger) animal).Bar(new Apple()).Should().Be("Tiger: Bar");
        }

        [Fact]
        public void Test3()
        {
            var tiger = new Tiger();
            tiger.Baz(new Apple()).Should().Be("Tiger: Baz"); // 有可选参数也能匹配上
            ((Animal)tiger).Baz(new Apple()).Should().Be("Animal: Baz");
        }

        [Fact]
        public void Test4()
        {
            var tiger = new Tiger();
            tiger.Quz(new List<Apple> {new()}).Should().Be("Tiger: Quz"); 
            //4.0以上版本支持协变和逆变，会是Tiger: Quz
            //但老版本不支持支持协变和逆变，会是Animal: Quz
        }
        
    }
}