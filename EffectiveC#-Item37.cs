using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_learning
{
    // 尽量采用惰性求值的方式来查询，而不要及早求值
    public class EffectiveC__Item37
    {
        private IEnumerable<int> AllNumbers()
        {
            var number = 0;
            while (number < int.MaxValue)
            {
                yield return number++;
            }
        }

        [Fact]
        public void OnlyGetTop10Numbers()
        {
            var answer = from number in AllNumbers() select number;
            var smallNumbers = answer.Take(10);
            Assert.Equal(10, smallNumbers.Count());
            Assert.Equal(45, smallNumbers.Sum());

            var answer2 = from number in answer select number + 1;
            var smallNumbers2 = answer2.Take(10);
            Assert.Equal(10, smallNumbers2.Count());
            Assert.Equal(55, smallNumbers2.Sum());

            // where, order by会需要处理序列的全部数据才能得出结果
            // var answer = from number in AllNumbers() where number < 10 select number;
        }

        private IEnumerable<Product> Products()
        {
            var number = 1;
            while (number < 10000001)
            {
                yield return new Product($"{number}", number++);
            }
        }

        [Fact]
        public void Compare()
        {
            // 1 order before filter
            var orderBefore =
                from p in Products()
                orderby p.UnitInStack descending
                where p.UnitInStack < 100
                select p;
            // 2 filter before order
            var filterBefore =
                from p in Products()
                where p.UnitInStack < 100
                orderby p.UnitInStack descending
                select p;

            DateTime beforDT1 = DateTime.Now;
            var products1 = orderBefore.ToList();
            DateTime afterDT1 = DateTime.Now;
            TimeSpan ts1 = afterDT1.Subtract(beforDT1);

            DateTime beforDT2 = DateTime.Now;
            var products2 = filterBefore.ToList();
            DateTime afterDT2 = DateTime.Now;
            TimeSpan ts2 = afterDT2.Subtract(beforDT2);

            Assert.True(ts2 < ts1);
        }
    }

    public class Product
    {
        public string Name { set; get; }
        public int UnitInStack { set; get; }

        public Product(string name, int number)
        {
            Name = name;
            UnitInStack = number;
        }
    }
}