using System;
using Xunit;

namespace csharp_learning
{
    // 用适当的方法初始化类中的静态成员
    public class EffectiveC__Item13
    {
        private void MakeNewHope()
        {
            var hope = new HopeNoError();
        }

        [Fact]
        public void DoSomeThing()
        {
            try
            {
                var class1 = new MakeSomeError();
            }
            catch (Exception)
            {
                // ignored
            }

            Assert.Throws<TypeInitializationException>(MakeNewHope);
        }
    }

    public class MySingleton
    {
        private static readonly MySingleton theOneAndOnly = new MySingleton();

        public static MySingleton TheOnly
        {
            get { return theOneAndOnly; }
        }

        private MySingleton()
        {
        }
    }

    // 初始化复杂可以如下写法
    public class MySingleton2
    {
        private static readonly MySingleton2 theOneAndOnly;

        // 每个类一个，不带参数
        // CLR自动调用
        static MySingleton2()
        {
            try
            {
                theOneAndOnly = new MySingleton2();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // 不要让异常脱出静态构造函数的范围，如果在外部被捕获
            // 由于CLR不会再次调用静态构造，该类型及派生类无法被正确初始化
        }

        public static MySingleton2 TheOnly
        {
            get { return theOneAndOnly; }
        }

        private MySingleton2()
        {
        }
    }

    // 使用静态初始化语句和静态构造函数为类中静态成员变量设定初始值

    public class MakeSomeError
    {
        public static string _message;

        static MakeSomeError()
        {
            _message = "ok";
            throw new ArgumentException();
        }
    }

    public class HopeNoError : MakeSomeError
    {
        public string _hopeMessage;

        public HopeNoError()
        {
            _hopeMessage = "hope";
        }
    }
}