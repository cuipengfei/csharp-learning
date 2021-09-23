using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    //尽量避免装箱与取消装箱这两种操作
    public class UnitTest09
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest09(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldCreateNewObjectWhenBoxing()
        {
            int number = 100;

            object o = number;

            number = 200;
            
            Assert.Equal(200, number);
            Assert.Equal(100, o);
        }

        // 注意那些会隐式转换成System.Object的地方。尽量不要在需要使用System.Object的地方直接使用值类型的值。
        [Fact]
        public void ShouldBoxHappenedWhenUsingValueTypeInConsoleLog()
        {
            int number1 = 100;
            int number2 = 200;
            
            _testOutputHelper.WriteLine($"A few numbers: {number1}, {number2}");
            _testOutputHelper.WriteLine($"A few numbers: {number1.ToString()}, {number2.ToString()}"); //推荐的做法
        }

        [Fact]
        public void ShouldNotUseReferenceWhenGiveAValueTypeInCollection()
        {
            var attendees = new List<Person>();
            Person p = new Person {Name = "Peter"};
            attendees.Add(p);

            Person p2 = attendees[0]; //attendees[0] 取出来的是p的拷贝；建议把值类型设计成不可变的类型
            p2.Name = "New Name";
            
            Assert.Equal("Peter", attendees[0].Name);
            Assert.Equal("New Name", p2.Name);
        }
        
        public struct Person
        {
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
    }
}