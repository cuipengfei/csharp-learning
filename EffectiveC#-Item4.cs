using System;
using Xunit;

namespace csharp_learning
{
    public class UnitTest4
    {
        // C# 6的新功能，interpolated strings，类似于JavaScript中的Template strings
        // https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/keywords/interpolated-strings?redirectedfrom=MSDN
        // have a try: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/exploration/interpolated-strings

        [Fact]
        public void UseInterpolatedStringInsteadOfStringFormat()
        {
            
            var s = "test";
            var str1 = string.Format("message is {0}", s);
            var str2 = $"message is {s}";
            Assert.Equal(str1, str2);

            var item = (Name: "eggplant", Price: 1.99m, perPackage: 3);
            var date = DateTime.Now;
            str1 = string.Format(@"On {0:d}, the price of {1:G} was {2:C2} per {3:G} items", date, item.Name, item.Price, item.perPackage);
            str2 = $@"On {date:d}, the price of {item.Name} was {item.Price:C2} per {item.perPackage} items";
            Assert.Equal(str1, str2);
        }

        [Fact]
        public void UseExpressionWithinInterpolatedString()
        {
            var a = 5;
            var b = 10;
            var str = $"result is {a+b}";
            
            Assert.Equal("result is 15", str);

            bool foo = false;
            str = $"test {(foo ? "foo is true" : "foo is false")}";
            Assert.Equal("test foo is false", str);
        }

        [Fact]
        public void UseAtAndDollar() {
            var name = "XiaoMing";
            var str = $@"Hello, ""{name}"". 
It's a pleasure to meet you\!";
            Assert.Equal("Hello, \"XiaoMing\". \n" +
"It's a pleasure to meet you\\!", str);
        }

    }
}
