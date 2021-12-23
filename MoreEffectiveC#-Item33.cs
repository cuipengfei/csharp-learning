using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace csharp_learning
{
    // 实现task的取消和进度报告
    public class MoreEffectiveCSharpItem33
    {
        public Task RunStep(string description)
        {
            return Task.Run(() =>
            {
                Console.Out.WriteLine(description);
                Thread.Sleep(2000);
            });
        }
        
        public async Task RunTask()
        {
            await RunStep("do step 1");
            await RunStep("do step 2");
            await RunStep("do step 3");
            await RunStep("do step 4");
        }

        public async Task RunTask(IProgress<(int, string)> progress)
        {
            progress?.Report((0, "Starting Task"));
            
            await RunStep("do step 1");
            progress?.Report((25, "done step 1"));

            await RunStep("do step 2");
            progress?.Report((50, "done step2"));
            
            await RunStep("do step 3");
            progress?.Report((75, "done step3"));
            
            await RunStep("do step 4");
            progress?.Report((100, "done step4"));
        }

        class ProgressReporter : IProgress<(int percent, string message)>
        {
            public void Report((int percent, string message) value)
            {
                Console.Out.WriteLine($"{value.percent}% completed: {value.message}");
            }
        }

        [Fact]
        public async Task TestProgressReport()
        {
            await RunTask(new ProgressReporter());
            // 执行结果：
            // 0% completed: Starting Task
            // do step 1
            // 25% completed: done step 1
            // do step 2
            // 50% completed: done step2
            // do step 3
            // 75% completed: done step3
            // do step 4
            // 100% completed: done step4
        }

        public async Task RunTask(CancellationToken cancellationToken)
        {
            await RunStep("do step 1");
            cancellationToken.ThrowIfCancellationRequested();
            await RunStep("do step 2");
            cancellationToken.ThrowIfCancellationRequested();
            await RunStep("do step 3 (assume: once step 3 is done, not allow to cancel task)");
            await RunStep("do step 4");
        }

        [Fact]
        public async Task TestCancellation()
        {
            var cts = new CancellationTokenSource();
            var task = RunTask(cts.Token);
            
            // 预计执行step 2时取消
            Thread.Sleep(3000);
            cts.Cancel();
            
            // 通过抛出异常TaskCanceledException，来报告任务被取消了
            Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        }
        
        // 将两者结合的常规的实现方法: 总是创建CancellationToken,只是不支持的时候就不会使用它
        public Task Process() => Process(new CancellationToken(), null);
        public Task Process(CancellationToken cancellationToken) => Process(cancellationToken, null);
        public Task Process(IProgress<(int, string)> progress) => Process(new CancellationToken(), progress);
        public async Task Process(CancellationToken cancellationToken, IProgress<(int, string)> progress)
        {
            progress?.Report((0, "Starting Task"));
            
            await RunStep("do step 1");
            cancellationToken.ThrowIfCancellationRequested();
            progress?.Report((25, "done step 1"));

            await RunStep("do step 2");
            cancellationToken.ThrowIfCancellationRequested();
            progress?.Report((50, "done step2"));
            
            await RunStep("do step 3");
            progress?.Report((75, "done step3"));
            
            await RunStep("do step 4");
            progress?.Report((100, "done step4"));
        }
    }
}