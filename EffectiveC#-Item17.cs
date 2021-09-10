using System;
using System.IO;

namespace csharp_learning
{
    // 实现标准的dispose模式
    public class EffectiveCItem17
    {
        // 如果类里包含非托管资源，需要实现IDisposable接口
        class Parent : IDisposable
        {
            private bool _alreadyDisposed = false; // 释放标记
            protected String path; // 托管资源
            protected StreamReader Reader; // 非托管资源

            public Parent()
            {
                path = @"./README.md";
                Reader = new StreamReader(path);
            }

            public string GetReadmeFileStr()
            {
                // 清理过后不允许再次访问public资源
                if (_alreadyDisposed)
                {
                    throw new ObjectDisposedException("Parent");
                }

                string res = "";
                while (!Reader.EndOfStream)
                {
                    res = res + Reader.ReadLine();
                }

                Console.Out.WriteLine(res);
                return res;
            }

            // 具体释放资源的工作委派给虚方法，子类可以重写
            protected virtual void Dispose(bool disposing)
            {
                // 如果资源已经被释放，就不再重复操作
                if (_alreadyDisposed) return;
                if (disposing)
                {
                    path = null;
                }
                Reader.Dispose();
                _alreadyDisposed = true;
            }

            // IDisposable接口只包含这一个方法，类的使用者需要及时调用该方法释放资源
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this); //阻止垃圾回收器重复清理
            }

            // 添加finalizer，防止使用者忘记调用Dispose()
            ~Parent()
            {
                Dispose(false);
            }
        }

        class Child: Parent
        {
            private bool hasDisposed = false;
            private StreamReader anotherUnmanagedResource = new StreamReader(@"./LICENSE");

            // 重写Dispose虚函数
            protected virtual void Dispose(bool disposing)
            {
                if (hasDisposed) return;
                if(disposing){}
                // 释放子类资源
                anotherUnmanagedResource.Dispose();
                // 释放父类资源
                base.Dispose(disposing);
                // 使用与父类不同的标志位
                hasDisposed = true;
            }

            ~Child()
            {
                Dispose(false);
            }
        }
    }
}