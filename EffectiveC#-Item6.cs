using System;
using Xunit;

namespace csharp_learning
{
    public class EffectiveC__Item6
    {
        public static string getParamName(Object personName)
        {
            if (personName is null)
            {
                var paramName = nameof(personName);
                return  $"{paramName} can't be null";
            }
            // 用到参数名字的时候不要硬编码
            // if (personName is null)
            // {
            //     return  "catName can't be null";
            // }

            return "ok";
        }

        [Fact]
        public void justTest()
        {
            Assert.Equal(getParamName(null),"personName can't be null");
            Assert.Equal(getParamName(new object()),"ok");
        }
    }
    
}