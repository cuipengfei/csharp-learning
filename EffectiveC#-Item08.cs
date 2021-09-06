using System;
using Xunit;

namespace csharp_learning
{
    public class EventHandlerExample
    {
        //  event 是一种特殊的delegate
        public event EventHandler Handler;

        //  等价于 delegate void MyEventHandler(object sender, System.EventArgs e);
        //  private MyEventHandler Handler;

        public static int counter = 0;
        class MyFirstJob
        {
            public void FooThing(EventHandlerExample eventHandlerExample)
            {
                // ... 
                // ...
                eventHandlerExample.Handler +=  this.TriggerByEvent;
            }
            void TriggerByEvent(object sender, System.EventArgs e)
            {
                counter++;
            }
        }
        
        class MySecondJob
        {
            public void BarSomething(EventHandlerExample eventHandlerExample)
            {
                // ... 
                // ...
                eventHandlerExample.Handler +=  this.TriggerByEvent;
            }
            
            public void TriggerByEvent(object sender, System.EventArgs e)
            {
                counter++;
            }
        }

        // 以调用delegate的方式写事件触发函数
        void OnEvent(System.EventArgs e)
        {
            // Handler?.Invoke(this, e);

            Handler?.Invoke(this,e);

        }

        void RaiseEvent()
        {
            EventArgs e = new EventArgs();
            
            OnEvent(e);
        }
        

        [Fact]
        public void EventTest()
        {
            EventHandlerExample eventHandlerExample = new EventHandlerExample();
            new MyFirstJob().FooThing(eventHandlerExample);
            new MyFirstJob().FooThing(eventHandlerExample);
            new MySecondJob().BarSomething(eventHandlerExample);
            
            eventHandlerExample.RaiseEvent();
            Assert.Equal(3, counter);
        }
    }
}