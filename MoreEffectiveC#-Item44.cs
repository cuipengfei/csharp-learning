using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    public class MoreEffectiveCSharpItem44
    {
        // 44. 通过动态编程技术更好地运用泛型参数的运行期类型
        // 不使用 Cast<> 做自定义类型转换，如有必要，使用 dynamic 编写自定义的转换。

        #region TestOutputHelper
        
        private readonly ITestOutputHelper _testOutputHelper;

        public MoreEffectiveCSharpItem44(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        #endregion
    
        [Fact]
        public void ShouldThrowInvalidCastExceptionWhenUseGenericCast()
        {
            void UseGenericCast()
            {
                var answers = GetSomeStrings().Cast<MyType>();
                // same as: var answers = from MyType answer in GetSomeStrings() select answer;
                
                foreach (var answer in answers)
                {
                    _testOutputHelper.WriteLine(answer.ToString());
                }

            }
            
            Assert.Throws<InvalidCastException>(UseGenericCast);
        }

        [Fact]
        public void ShouldCastSuccessfullyWhenUseSelectFunction()
        {
            void UseSelectFunction()
            {
                var answers = GetSomeStrings().Select(s => (MyType) s);
                // same as: var answers = from answer in GetSomeStrings() select (MyType)answer;
                foreach (var answer in answers)
                {
                    _testOutputHelper.WriteLine(answer);
                }
            }
            
            UseSelectFunction();
        }

        [Fact]
        public void ShouldCastSuccessfullyWhenUsePrivateConvert()
        {
            void UsePrivateConvert()
            {
                var convertedSequence = GetSomeStrings().Convert<MyType>();

                foreach (var item in convertedSequence)
                {
                    _testOutputHelper.WriteLine(item);
                }
            }
            
            UsePrivateConvert();
        }

        private static IEnumerable<string> GetSomeStrings()
        {
            var strings = new List<string>
            {
                "1",
                "2"
            };
            return strings;
        }
    }

    public static class Item44UseDynamic
    {
        public static IEnumerable<TResult> Convert<TResult>(this System.Collections.IEnumerable sequence)
        {
            foreach (object item in sequence)
            {
                dynamic coercion = item;
                yield return (TResult) coercion;
            }
        }
    }

    public class MyType
    {
        public string StringMember { get; init; }

        public static implicit operator string(MyType exampleString) => exampleString.StringMember;

        public static implicit operator MyType(string exampleString) => new() {StringMember = exampleString};
    }
}