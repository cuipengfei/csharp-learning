using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace csharp_learning
{
    // 理解查询表达式与方法调用之间的映射关系
    public class People
    {
        public readonly int Age;
        public readonly string LastName;
        public readonly string FirstName;
        public readonly string City;

        public People(int age, string lastName, string firstName, string city)
        {
            Age = age;
            LastName = lastName;
            FirstName = firstName;
            City = city;
        }

        public static List<People> getPeoples()
        {
            List<People> peoples = new List<People>
            {
                new People(22, "Wang", "Feng", "Xian"),
                new People(18, "Cai", "Xukun", "Xian"),
                new People(55, "Cai", "Ming", "Beijing"),
                new People(50, "Liu", "Huan", "Beijing"),
                new People(48, "Li", "Keqin", "Chengdu")
            };
            return peoples;
        }
    }

    public class UnitTest36
    {
        [Fact]
        public void ShouldReturnNumbersWhenFilterGreater3()
        {
            var numbers = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var smallNumbers = from n in numbers
                where n < 3
                select n;

            // 可以省略Select，因为Select是要在上一条表达式的返回结果中做选择；.Select(n => n)
            // var smallNumbers = numbers.Where(n => n < 3);

            Assert.Equal(new[] {0, 1, 2}, smallNumbers);
        }

        //Select 的应用场景：将输入的转换为其他元素或转换成另一种类型
        [Fact]
        public void ShouldReturnSquareValue()
        {
            var numbers = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var squareNumbersFromQueryExpression = from n in numbers
                where n < 3
                select n * n;

            var squareNumbers = numbers
                .Where(n => n < 3)
                .Select(n => n * n);
            
            Assert.Equal(new[] {0, 1, 4}, squareNumbers);
            Assert.Equal(new[] {0, 1, 4}, squareNumbersFromQueryExpression);

            var squaresFromQueryExpression = from n in numbers
                where n < 3
                select new {Number = n, Square = n * n};

            var squares = numbers
                .Where(n => n < 3)
                .Select(n => new {Number = n, Square = n * n});

            var expectedResult = new[]
            {
                new {Number = 0, Square = 0},
                new {Number = 1, Square = 1},
                new {Number = 2, Square = 4}
            }.ToList();

            squaresFromQueryExpression.ToList().Should().Equal(expectedResult);
            squares.ToList().Should().Equal(expectedResult);
        }

        [Fact]
        public void ShouldReturnPersonWithExpectedOrder()
        {
            var peoples = People.getPeoples();

            // var orderedPeople = from p in peoples
            //     where p.Age > 20
            //     orderby p.FirstName descending , p.LastName, p.Age
            //     select p;

            var orderedPeople = peoples.Where(p => p.Age > 20)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ThenBy(p => p.Age);
            // IOrderedEnumerable<People> result = Enumerable.Empty<People>().OrderBy(x => x.Age);
            // Assert.Equal(result, orderedPeople);
        }

        [Fact]
        public void TestForGroupBy()
        {
            var peoples = People.getPeoples();
            var results = from p in peoples
                group p by p.City
                into c
                select new
                {
                    City = c.Key,
                    Size = c.Count()
                };

            var result2 = from c in
                    from p in peoples group p by p.City
                select new
                {
                    City = c.Key,
                    Size = c.Count()
                };

            var result3 = peoples.GroupBy(p => p.City)
                .Select(c => new
                {
                    City = c.Key,
                    Size = c.Count()
                });

            var expectedResult = new[]
            {
                new {City = "Xian", Size = 2},
                new {City = "Beijing", Size = 2},
                new {City = "Chengdu", Size = 1}
            };

            results.ToList().Should().Equal(expectedResult);
            result2.ToList().Should().Equal(expectedResult);
            result3.ToList().Should().Equal(expectedResult);
        }


        [Fact]
        public void TestSelectMany()
        {
            int[] odds = {1, 3, 5};
            int[] evens = {2, 4, 6};

            var pairs = from oddNumber in odds
                from evenNumber in evens
                select new
                {
                    oddNumber,
                    evenNumber,
                    Sum = oddNumber + evenNumber
                };

            var pairs2 = odds.SelectMany(_ => evens, (oddNumber, evenNumber) =>
                new
                {
                    oddNumber,
                    evenNumber,
                    Sum = oddNumber + evenNumber
                });

            var expectedResult = new[]
            {
                new {oddNumber = 1, evenNumber = 2, Sum = 3},
                new {oddNumber = 1, evenNumber = 4, Sum = 5},
                new {oddNumber = 1, evenNumber = 6, Sum = 7},
                new {oddNumber = 3, evenNumber = 2, Sum = 5},
                new {oddNumber = 3, evenNumber = 4, Sum = 7},
                new {oddNumber = 3, evenNumber = 6, Sum = 9},
                new {oddNumber = 5, evenNumber = 2, Sum = 7},
                new {oddNumber = 5, evenNumber = 4, Sum = 9}, 
                new {oddNumber = 5, evenNumber = 6, Sum = 11},
            };

            pairs.ToList().Should().Equal(expectedResult);
            pairs2.ToList().Should().Equal(expectedResult);
        }

        //todo: Join GroupJoin;
    }
}