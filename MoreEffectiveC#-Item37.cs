using System;
using System.Threading;
using Xunit;

namespace csharp_learning
{
    // 使用线程池替代自己创建和管理线程
    // 1. 线程池会重用线程，减少资源浪费；2. 线程池可以管理资源和线程状态
    public class MoreEffectiveCSharpItem37
    {
        private static void ThreadPoolExample(int numThreads)
        {
            for (int i = 0; i < numThreads; i++)
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    Console.WriteLine("{0}:第{1}个线程执行", DateTime.Now, x);
                    Thread.Sleep(5000);
                }, i);
            }
        }
        
        [Fact]
        public void Test(){
            ThreadPoolExample(6);
            Thread.Sleep(6000);
        }
    }
}