using Xunit;
using Xunit.Abstractions;

namespace System.Threading.Tasks
{
    public class MoreEffectiveC__Item31
    {
        private static ITestOutputHelper _testOutputHelper;

        public MoreEffectiveC__Item31(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private static async Task<SynchronizationContext> GetStringAsync1()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync1方法启动");
            SynchronizationContext synchronizationContext = await GetStringAsync2();
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync1方法结束");
            return synchronizationContext;
        }

        private static async Task<SynchronizationContext> AnotherGetStringAsync1()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync1方法启动");
            SynchronizationContext synchronizationContext = await GetStringAsync2();
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync1方法结束");
            return synchronizationContext;
        }
        
        private static async Task<SynchronizationContext> ThirdGetStringAsync1()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync1方法启动");
            SynchronizationContext synchronizationContext = await GetStringAsync2();
            var synchronizationContext2 = SynchronizationContext.Current; 
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync1方法结束");
            return synchronizationContext2;
        }
        
        private static async Task<SynchronizationContext> ForthGetStringAsync1()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync1方法启动");
            SynchronizationContext synchronizationContext = await GetStringAsync2().ConfigureAwait(continueOnCapturedContext:false);
            var synchronizationContext2 = SynchronizationContext.Current; 
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync1方法结束");
            return synchronizationContext2;
        }
        
        private static async Task<SynchronizationContext> FifthGetStringAsync1()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync1方法启动");
            SynchronizationContext synchronizationContext = await GetStringAsync2().ConfigureAwait(continueOnCapturedContext:false);
            var synchronizationContext2 = SynchronizationContext.Current; 
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync1方法结束");
            return synchronizationContext2;
        }
        
        private static async Task<SynchronizationContext> SixthGetStringAsync1()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync1方法启动");
            var synchronizationContext2 = SynchronizationContext.Current; 
            SynchronizationContext synchronizationContext = await GetStringAsync2().ConfigureAwait(continueOnCapturedContext:false);
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync1方法结束");
            return synchronizationContext2;
        }
        private static async Task<SynchronizationContext> GetStringAsync2()
        {
            _testOutputHelper.WriteLine(">>>>>>>>GetStringAsync2方法启动");
            await GetStringFromTask();
            var synchronizationContext = SynchronizationContext.Current; 
            _testOutputHelper.WriteLine("<<<<<<<<GetStringAsync2方法结束");
            return synchronizationContext;
        }

        private static Task GetStringFromTask()
        {
            _testOutputHelper.WriteLine(">>>>GetStringFromTask方法启动");
            Task task = new Task(() =>
            {
                _testOutputHelper.WriteLine(">>任务线程启动");
                Thread.Sleep(1000);
                _testOutputHelper.WriteLine("<<任务线程结束");
            });
            task.Start();
            _testOutputHelper.WriteLine("<<<<GetStringFromTask方法结束");
            return task;
        }

        [Fact]
        public async void ShouldEqualFromSameTask()
        {
            var firstResult = await GetStringAsync1();
            var SecondResult = await AnotherGetStringAsync1();
            Assert.Equal(firstResult,SecondResult);
        }

        [Fact]
        public async void ShouldAppearDifferentContext()
        {
            var firstResult = await ThirdGetStringAsync1();
            var secondResult = await ForthGetStringAsync1();
            Assert.NotEqual(firstResult,secondResult);
        }
        
        [Fact]
        public async void ShouldAppearSameContext()
        {
            var firstResult = await ForthGetStringAsync1();
            var secondResult = await FifthGetStringAsync1();
            Assert.Equal(firstResult,secondResult);
            Assert.Null(firstResult);
        }

        [Fact]
        public async void ShouldAppearDifferentContextInDifferentLocation()
        {
            var firstResult = await FifthGetStringAsync1();
            var secondResult = await SixthGetStringAsync1();
            Assert.NotEqual(firstResult,secondResult);
            Assert.Null(firstResult);
            Assert.NotNull(secondResult);
        }
    }
}