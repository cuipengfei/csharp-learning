using System;
using Xunit;

namespace csharp_learning
{
    public class MoreEffectiveC__Item16
    {
        [Fact]
        public void ShouldHandle()
        {
            ConsoleLogger test = new ConsoleLogger();
            Logger.Singleton.AddMsg(3, "test");
            Assert.Equal(3, Num.num);
        }
    }

    public class Logger
    {
        static Logger()
        {
            Singleton = new Logger();
        }

        private Logger()
        {
        }

        public static Logger Singleton { get; }

        // Define the event
        public event EventHandler<LoggerEventArgs> Log;

        // Add a message, and log it
        public void AddMsg(int priority, string msg) => Log?.Invoke(this, new LoggerEventArgs(priority, msg));
    }

    public class LoggerEventArgs
    {
        public LoggerEventArgs(int priority, string msg)
        {
            Priority = priority;
            Msg = msg;
        }

        public int Priority { get; set; }
        public string Msg { get; set; }
    }

    public class ConsoleLogger
    {
        public ConsoleLogger() =>
            Logger.Singleton.Log += (sender, msg) =>
                Num.num += msg.Priority;
    }

    static class Num
    {
        public static int num = 0;
    }
}