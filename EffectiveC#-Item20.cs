using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Xunit;

namespace csharp_learning
{
    // 通过IComparable<T>及IComparer<T>定义顺序关系
    public class EffectiveCItem20
    {
        public struct Person : IComparable<Person>, IComparable
        {
            public string Name { get; }
            public int Age { get; }

            public Person(string name, int age)
            {
                Name = name;
                Age = age;
            }

            // IComparable<T>
            public int CompareTo(Person other)
            {
                return String.CompareOrdinal(Name, other.Name);
            }

            // IComparable, 兼容旧式API
            public int CompareTo(object? obj)
            {
                // 需要转换参数类型
                if (!(obj is Person))
                    throw new ArgumentException("Argument is not a Person", "obj");

                Person other = (Person) (obj);
                return CompareTo(other);
            }

            // 重载运算关系
            public static bool operator <(Person left, Person right) => left.CompareTo(right) < 0;
            public static bool operator >(Person left, Person right) => left.CompareTo(right) > 0;
            public static bool operator ==(Person left, Person right) => left.CompareTo(right) == 0;
            public static bool operator !=(Person left, Person right) => !(left == right);

            

        }

        Person john = new Person("John", 30);
        Person tom = new Person("Tom", 20);

        [Fact]
        public void ComparableTest()
        {
            Assert.True(john.CompareTo(tom) < 0);
            Assert.Throws<ArgumentException>(() => john.CompareTo(new Dog()));
        }

        [Fact]
        public void OverloadOperatorTest()
        {
            Assert.True(john < tom);
            Assert.True(tom > john);
            Assert.True(john == new Person("John", 40));
            Assert.True(john != tom);
        }

        // 定义其他比较方法
        [Fact]
        public void ComparisonTest()
        {
            Person[] array = {john, tom};
            Comparison<Person> compareByAge = (left, right) => left.Age - right.Age;

            Array.Sort(array, compareByAge);
                
            Person[] expected = {tom, john};
            Assert.Equal(expected, array);
        }
        
        class AgeComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y) => x.Age - y.Age;
        }
        
        // 定义其他比较方法，较老的方式
        [Fact]
        public void ComparerTest()
        {
            Person[] array = {john, tom};

            Array.Sort(array, new AgeComparer());
                
            Person[] expected = {tom, john};
            Assert.Equal(expected, array);
        }
    }
}