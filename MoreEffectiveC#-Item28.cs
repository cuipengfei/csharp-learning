using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    public class MoreEffectiveC__Item28
    {
        // 28. 不要编写返回值类型为 void 的异步方法
        
        private readonly ITestOutputHelper _testOutputHelper;

        public MoreEffectiveC__Item28(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void Run()
        {
            AppDomain.CurrentDomain.UnhandledException += (@object, args) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                _testOutputHelper.WriteLine("Exception happened! - " + args.ExceptionObject);
            };
            
            CallAsyncTaskMethod().GetAwaiter().GetResult();
            // CallAsyncVoidMethod();
            // Thread.Sleep(4000);
            
            _testOutputHelper.WriteLine("End.");
        }

        private static async Task CallAsyncTaskMethod()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                throw new Exception();
            });
        }

        private static async void CallAsyncVoidMethod()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                throw new Exception();
            });
        }

        private static async void OnCommand(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    // do something with error...
                });
            }
            catch (Exception exception) when (LogMessage(new object(), exception)) // return false 以后 catch 不会执行会 throw 终止程序
            {
                Console.WriteLine(exception);
            }
        }

        private static bool LogMessage(object o, Exception ex)
        {
            // log a message
            return false;
        }
    }
}