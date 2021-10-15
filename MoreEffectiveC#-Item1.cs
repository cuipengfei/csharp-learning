using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace csharp_learning
{
    // 应该尽量“使用属性而不是可直接访问的数据成员”。因为属性具有修改的便捷性，多线程的支持等等
    public class UnitTest1_MoreEffective
    {
        public class NotSuggestedWay
        {
            public string Name;
        }

        public class SuggestedWay
        {
            private string _name;

            public string Name
            {
                get => _name;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException(
                            "Name cannot be blank",
                            nameof(Name)
                        );
                    }

                    _name = value;
                }
            }
        }
        
        public class ReadonlyExample
        {
            public string ReadOnlyName { get; init; } // same with { get; private set;}
            
            public ReadonlyExample(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException(message: "Cannot be blank", paramName: nameof(name));
                ReadOnlyName = name;
            }

            public void ChangeName(string name)
            {
                // Generates CS0200: Property or indexer cannot be assigned to -- it is read only
                // ReadOnlyName = name;
            }
        }

        public class VirtualExample
        {
            public virtual string Name
            {
                get;
                set;
            }
        }

        // 属性不仅适用于简单的数据字段，如果某个类型要在其他接口中发布能够用索引来访问的内容，那么就可以创建索引器，相当于带有参数的属性
        public class ParameterPropertyExample
        {
            // 若参数是整数的一维索引器，则可以参与数据绑定 => 通过索引器的形式发布序列
            public int this[int index]
            {
                get => theValues[index];
                set => theValues[index] = value;
            }

            private int[] theValues = new int[100];
            
            //若参数不是整数的一维索引器，则可以用来定义映射关系  => 通过索引器的形式发布字典
            private Dictionary<string, SuggestedWay> addressValues = new Dictionary<string, SuggestedWay>();
            public SuggestedWay this[string name]
            {
                get => addressValues[name];
                set => addressValues[name] = value;
            }
            
        }
        
        public class SupportForMultipleThread
        {
            private object syncHandle = new object();

            private string _name;
            public string Name
            {
                get
                {
                    lock (syncHandle)
                    {
                        return _name;
                    }
                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException(
                            "Name connot be blank",
                            nameof(Name)
                        );
                    }

                    lock (syncHandle)
                    {
                        _name = value;
                    }
                }
            }
        }
        
        public class ConstAndReadonly
        {
            public static readonly Program program = new Program();
            public const float CentimetersPerInch = 2.54F;
        }

        [Fact]
        void Test()
        {
            var notSuggestedWay = new NotSuggestedWay();
            notSuggestedWay.Name = "abc";
            var notSuggestedWayName = notSuggestedWay.Name;

            var suggestedWay = new SuggestedWay();
            suggestedWay.Name = "abc";
            var suggestedWayName = suggestedWay.Name; // 在C#语言中， 属性这种元素可以像数据成员一样被访问， 但它们其实是通过方法来实现的

            ParameterPropertyExample example = new ParameterPropertyExample();
            example[5] = 1;
            var value = example[5];
            example["first"] = suggestedWay;

            Assert.Equal(1, example[5]);
            Assert.Equal("abc", example["first"].Name);
        }
        
        
    }
}