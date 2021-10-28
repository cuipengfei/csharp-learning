using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace csharp_learning
{
    //Item 6: Ensure That Properties Behave Like Data
    //这一节讲的东西比较简单，主要建议属性内不应该做复杂操作，如果真的需要做复杂操作的话就写成方法。
    //因为使用者会期待访问属性时更像读写数据，而不像调用方法，虽然说属性是由方法实现的。

    //其中用到的一段实例代码用到的lazy比较值得学习
    public class MoreEffectiveCSharpItem6
    {

        private int time = 0;
        private string CreateAString()
        {
            time++;
            return "hello" + time;
        }

        [Fact]
        public void LazyOnlyRunsOneTime()
        {
            var lazyS = new Lazy<String>(CreateAString);

            Assert.Equal(0, time); //new Lazy不会导致方法执行

            Assert.Equal("hello1", lazyS.Value);//第1次问它要value，会执行一次方法
            Assert.Equal(1, time);

            Assert.Equal("hello1", lazyS.Value); //第2次问它要value，并不会再执行一次方法
            Assert.Equal(1, time);
        }
    }
}