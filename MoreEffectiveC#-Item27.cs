using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace csharp_learning
{
    // 使用async方法来做异步工作
    public class MoreEffectiveCSharpItem27
    {
        // 在C#支持async 和 await之前，传统方法是使用回调函数来处理异步任务的返回
        public void DoSomeAsyncTask(Action callback)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Start processing async task");
                Thread.Sleep(5000);
                callback();
            });
        }
        
        public void UseCallbackToProcessAsyncTask()
        {
            DoSomeAsyncTask(() =>Console.WriteLine("Async task finished"));
            Console.WriteLine("Do something when processing async task");
        }
        // 执行结果：
        // Do something when processing async task
        // Start processing async task
        // Async task finished


        // 使用async方法后，处理异步任务变得更加简单。可以向写同步方法一样去写异步算法的逻辑
        // 一个Task可以有多个await
        public async Task CallAsyncMethod()
        {
            // 模拟一场考试开始
            Console.WriteLine("老师发卷");
            Task waitForBell = Task.Run(() =>
            {
                Console.WriteLine("等待开考铃声。。。");
                Thread.Sleep(5000);
                Console.WriteLine("开考铃声响起");
            });
            Task end = Task.WhenAll(new List<int> {1, 2, 3, 4, 5}.Select(async (i) =>
            {
                await waitForBell;
                Console.WriteLine($"考生{i}开始答题");
                Thread.Sleep(5000);
                Console.WriteLine($"考生{i}交卷");
            }));
            Console.WriteLine("监考老师宣讲考场规则");
            await end;
        }
        // 执行结果： 
        // 老师发卷
        // 等待开考铃声。。。
        // 监考老师宣讲考场规则
        //
        // 开考铃声响起
        // 考生2开始答题
        // 考生5开始答题
        // 考生3开始答题
        // 考生4开始答题
        // 考生1开始答题
        //
        // 考生2交卷
        // 考生5交卷
        // 考生3交卷
        // 考生4交卷
        // 考生1交卷
        
        
        
        // async method具体的实现机制：简单来说编译器帮我将其转化成回调函数的形式
        // 编译器帮助我们构造一个 数据结构 和一个 委托
        // 数据结构保存的是局部变量 => SynchronizationContext.Currency
        // 委托存放的是await之后的代码指令
        //     不同类型的应用，async task执行完之后，触发后续工作的方式也会不同
        //     比如：GUI application会使用Dispatcher，web application会使用ThreadPool.QueueUserWorkItem

        // 如果async task出现异常，异常会在执行await后续工作时被抛出
        [Fact]
        public async Task ShouldNotThrowExceptionIfNotAwait()
        {
            Task task = Task.Run(() =>
            {
                throw new Exception("error");
            });
            
            Thread.Sleep(100);
            Assert.True(task.IsFaulted);

            await Assert.ThrowsAnyAsync<Exception>(async () => await task);
        }
    }
}