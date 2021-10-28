using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

/*
 * 在迭代器与异步方法中定义局部函数，以便尽早的报错*
*/

namespace Xunit
{
    /*
     * Tip1: 调用迭代器方法的时候，程序不会立刻抛出ArgumentException异常，而是要等到对该方法所返回的序列进行列举的时候才会抛出
     * 在大规模的项目中，由于创建迭代器的语句与列举迭代器的语句可能写在不同的方法乃至不同的类中，这样的问题很难排查
     */
    public class IteratorProcessorBadExample
    {
        public IEnumerable<T> GenerateSample<T>(IEnumerable<T> sequence, int sampleFrequency)
        {
            if (sequence == null)
                throw new ArgumentException("Souce sequence cannot be null", paramName: nameof(sequence));
            if (sampleFrequency < 1)
                throw new ArgumentException("Sample frequency must be a positive integer",
                    paramName: nameof(sampleFrequency));
            int index = 0;
            foreach (T item in sequence)
            {
                if (index++ % sampleFrequency == 0)
                {
                    yield return item;
                }
            }
        }
    }

    /*
    * Tip2:  如果想让某些代码尽早执行，可以考虑将原方法拆成两个部分，将纯粹的执行逻辑方法实现方法中去做
    */
    public class IteratorProcessorGoodExample
    {
        public IEnumerable<T> GenerateSample<T>(IEnumerable<T> sequence, int sampleFrequency)
        {
            // "包装器方法（wrapper method）"
            if (sequence == null)
                throw new ArgumentException("Souce sequence cannot be null", paramName: nameof(sequence));
            if (sampleFrequency < 1)
                throw new ArgumentException("Sample frequency must be a positive integer",
                    paramName: nameof(sampleFrequency));

            return GenerateSampleImpl();

            IEnumerable<T> GenerateSampleImpl() // "实现方法" - 局部函数：好处在于实现方法只能从包装器中得到调用；局部变量可以直接访问
            {
                int index = 0;
                foreach (T item in sequence)
                {
                    if (index++ % sampleFrequency == 0)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    public class AsyncProcessorBadExample
    {
        public async Task<string> LoadSomething(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException(message: "This must be a valid user", paramName: nameof(userName));
            }

            var task = await Task.Run(() =>
            {
                Thread.Sleep(5000);
                return $"Hello {userName}";
            });
            return task;
        }
    }

    public class AsyncProcessorGoodExample
    {
        public Task<string> LoadSomething(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException(message: "This must be a valid user", paramName: nameof(userName));
            }

            return LoadSomethingImpl();

            async Task<string> LoadSomethingImpl()
            {
                var task = await Task.Run(() =>
                {
                    Thread.Sleep(5000);
                    return $"Hello {userName}";
                });
                return task;
            }
        }
    }

    public class Test
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Test(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }


        [Fact]
        public void TestIteratorBadExample()
        {
            var processor = new IteratorProcessorBadExample();
            var exception = Record.Exception(() => processor.GenerateSample(new int[] {1, 2, 3, 4, 5}, -8));

            Assert.Null(exception);

            var generated = processor.GenerateSample(new int[] {1, 2, 3, 4, 5}, -8);

            Assert.Throws<ArgumentException>(() =>
            {
                foreach (var item in generated)
                    _testOutputHelper.WriteLine(item.ToString());
            });
        }

        [Fact]
        public async Task TestAsyncBadExample()
        {
            var throwException = false;
            var processor = new AsyncProcessorBadExample();
            try
            {
                processor.LoadSomething("");
            }
            catch (Exception e)
            {
                throwException = true;
            }
            Assert.False(throwException);

            try
            {
                await processor.LoadSomething("");
            }
            catch (Exception e)
            {
                throwException = true;
            }
            Assert.True(throwException);
        }
        
        
        [Fact]
        public void TestIteratorGoodExample()
        {
            var processor = new IteratorProcessorGoodExample();

            Assert.Throws<ArgumentException>(() =>
                processor.GenerateSample(new int[] {1, 2, 3, 4, 5}, -8));
        }
        
        [Fact]
        public void TestAsyncGoodExample()
        {
            var throwException = false;
            var processor = new AsyncProcessorGoodExample();
            try
            { 
                processor.LoadSomething("");
            }
            catch (Exception e)
            {
                throwException = true;
            }
            Assert.True(throwException);
        }
    }
}