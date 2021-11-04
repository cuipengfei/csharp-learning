using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_learning
{
    // 优先考虑定义并实现接口，而不是继承
    public class MoreEffectiveCSharpItem14
    {
        /**
         * 抽象基类：描述的是类别上的从属关系，A继承B，说明A是一种特殊的B；
         * 接口：描述的是行为上的相似关系，A实现了B，说明Ade行为像B；
         */
        
        /**
         * 接口：无代码实现和具体数据成员，可以针对接口创建扩展方法，使得该接口看起来像定义了这些方法；
         * 抽象基类：可以提供代码实现，通过派生子类实现代码复用；
         */
        [Fact]
        public void UsingCustomExtensionMethodOfInterface()
        {
            IEnumerable<string> data = new[]{"a", "b", "c"};
            var result = new List<string>();
            data.ForMyAll(n => result.Add(n.ToString()));
            // data.ToArray()
            
            Assert.Equal(3, result.Count);
        }

        /**
         * 添加新的方法：
         * 抽象基类：在基类中添加之后，会应用到所有的派生类中；相当于把一个新功能应用到所有的子类中；
         * 接口：不能直接在接口中添加新的行为，否则需要对所有的实现类都新增实现；可以从原接口中继承一个新的接口，并发新的行为添加到新的接口中
         *
         * 总结：用接口还是抽象类取决于抽象机制是否需要不断变化；接口是固定的，基类是随时变化的；
         */

        /**
         * 👆🏻 两种思想是可以结合使用的，将基本功能定义在接口中，并在其他类中，增加该接口的扩展方法；比如IEnumerble<T>与System.Linq.Enumerable类的关系；
         * 看起来是定义在接口中的方法，但实际是放在了扩展方法中，接口中只定义了基本的功能；
         */
        [Fact]
        public void UsingExtensionMethodOfInterface()
        {
            var weatherData = from item in new WeatherDataStream()
                where item.Temperature > 10
                select item;

            // new WeatherDataStream().Where(item => item.Temperature > 10);
            Assert.True(weatherData.Count() == 100);
        }



        /**
         * 参数和返回值得类型也可以声明为接口类型；同一个接口可以由多个互不相关的类型来实现，使得接口的设计更灵活
         */

        //参数类型为接口
        public static void PrintCollection<T>(IEnumerable<T> collection)
        {
            foreach (T o in collection)
            {
                Console.WriteLine(o.ToString());
            }
        }
        
        public static void PrintCollection(IEnumerable collection)
        {
            foreach (object o in collection)
            {
                Console.WriteLine(o.ToString());
            }
        }
        
        public static void PrintCollection(WeatherDataStream collection)
        {
            foreach (object o in collection)
            {
                Console.WriteLine(o.ToString());
            }
        }
        
        //用接口定义方法的返回类型
        //👇🏻 bad
        public List<WeatherData> DataSequence => sequence;
        private List<WeatherData> sequence = new();

        // good -> WeatherDataStream()
    }

    public class WeatherDataStream : IEnumerable<WeatherData>
    {
        public IEnumerator<WeatherData> GetEnumerator() => getElements();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<WeatherData> getElements()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
                yield return new WeatherData() { Temperature = random.Next(11, 30)};
        }
    }

    public class WeatherData
    {
        public float Temperature { get; set; }
        
    }

    public static class Extensions
    {
        public static void ForMyAll<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
            {
                action(item);
            }
        }
    }
}