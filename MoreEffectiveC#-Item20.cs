using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace csharp_learning
{

    // event可以避免编译时的耦合
    //但是要注意运行时产生的耦合
    //因为事件的订阅者之间默认是没有沟通机制的（除非你自己做了）

    public class Args
    {
        public int Percent { get; set; }

        public bool Continue { get; set; }
    }

    public class SomeWorker
    {
        public event EventHandler<Args> OnProgress;
        public void DoLotsOfStuff()
        {
            for (int i = 0; i < 100; i++)
            {
                SomeWork();
                Args args = new Args();
                args.Continue = true;
                args.Percent = i;

                OnProgress?.Invoke(this, args);
                if (!args.Continue)
                    return;//退出
            }
        }

        private void SomeWork()
        {
            //假装在做事
        }
    }

    public class MoreEffectiveCSharpItem20
    {
        [Fact]
        public void ExampleOfEventListenersCoupling()
        {
            int runCount1 = 0;
            int runCount2 = 0;
            var worker = new SomeWorker();

            worker.OnProgress += (sender, args) =>
            {
                //接收到进度更新之后做事
                bool checkFailed = args.Percent > 50;//假装某种检查失败了
                if (checkFailed)
                {
                    args.Continue = false;
                }
                runCount1++;
            };

            worker.OnProgress += (sender, args) =>
            {
                //接收到进度更新之后做事
                bool checkOk = args.Percent > 50;//假装某种检查ok
                if (checkOk)
                {
                    args.Continue = true;
                }
                runCount2++;
            };

            worker.DoLotsOfStuff();

            Assert.Equal(100, runCount1);//第一个事件订阅者其实是希望50之后就不跑了
            Assert.Equal(100, runCount2);//不过第二个事件订阅者希望50之后要继续跑
            //第二个后执行，于是反复推翻第一个订阅者的决定
            //二者之间是没有语言和框架级别提供的协调机制的，这些问题需要开发者自己处理
        }
    }
}