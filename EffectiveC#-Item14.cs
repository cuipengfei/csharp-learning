using System;
using System.Buffers.Binary;
using Xunit;

namespace csharp_learning
{
    // 尽量删除重复的初始化逻辑
    public class EffectiveCItem14
    {
        class Item14TestClassOne
        {
            private readonly string[] _coll;
            private readonly string _name;

            public Item14TestClassOne() :
                this(0, "")
            {
            }

            public Item14TestClassOne(int initialCount) : this(initialCount, string.Empty)
            {
            }

            public Item14TestClassOne(string name) : this(0, name)
            {
            }

            public Item14TestClassOne(int initialCount, string name)
            {
                _coll = (initialCount > 0) ? new string[initialCount] : Array.Empty<string>();
                this._name = name;
            }

            public int GetCount()
            {
                return _coll.Length;
            }

            public string GetName()
            {
                return _name;
            }
        }

        [Fact]
        public void Test1()
        {
            var item1 = new Item14TestClassOne();
            Assert.Equal(0, item1.GetCount());
            Assert.Equal("", item1.GetName());

            var item2 = new Item14TestClassOne(1);
            Assert.Equal(1, item2.GetCount());

            var item3 = new Item14TestClassOne(2, "hello");
            Assert.Equal(2, item3.GetCount());
            Assert.Equal("hello", item3.GetName());

            var item4 = new Item14TestClassOne("hello");
            Assert.Equal(0, item4.GetCount());
            Assert.Equal("hello", item4.GetName());
        }

        class Item14TestClassTwo
        {
            private string[] coll;

            private string name;

            // 作为参数传入new()约束的泛型类或者方法时，必须写明无参构造方法
            public Item14TestClassTwo() : this(1, string.Empty)
            {
            }

            // 只给name会报错
            // 默认参数只能用编译器常量，所以不能用静态参数 string.Empty
            public Item14TestClassTwo(int initialCount = 1, string name = "")
            {
                coll = (initialCount > 0) ? new string[initialCount] : Array.Empty<string>();
                this.name = name;
            }

            public int GetCount()
            {
                return coll.Length;
            }

            public string GetName()
            {
                return name;
            }
        }

        [Fact]
        public void Test2()
        {
            var item1 = new Item14TestClassTwo();
            Assert.Equal("", item1.GetName());
            Assert.Equal(1, item1.GetCount());

            var item2 = new Item14TestClassTwo(2);
            Assert.Equal(2, item2.GetCount());
            Assert.Equal("", item2.GetName());

            var item3 = new Item14TestClassTwo(3, "haveName");
            Assert.Equal(3, item3.GetCount());
            Assert.Equal("haveName", item3.GetName());

            // var item4 = new Item14TestClassTwo("why_error");
        }

        // 默认值的参数也有缺点，参数和默认值也会成为接口的一部分，修改完之后需要调用方重新编译才能更新默认值，重载的方法更能适应变化


        // 公共方法
        class Item14TestClassThree
        {
            // 无法设置为 readonly, readonly只能在构造函数初始化
            private string[] coll;
            private string name;

            public Item14TestClassThree()
            {
                commonConstructor(0, "");
            }

            public Item14TestClassThree(int initialCount)
            {
                commonConstructor(initialCount, "");
            }


            public Item14TestClassThree(int initialCount, string name)
            {
                commonConstructor(initialCount, name);
            }

            private void commonConstructor(int initialCount, string name)
            {
                coll = (initialCount > 0) ? new string[initialCount] : Array.Empty<string>();
                this.name = name;
            }
        }


        // 链式调用
        class Item14TestClassFour
        {
            private readonly string[] coll;
            private readonly string name;

            public Item14TestClassFour() : this(0, "")
            {
            }

            public Item14TestClassFour(int initialCount) : this(initialCount, "")
            {
            }

            public Item14TestClassFour(int initialCount, string name)
            {
                coll = (initialCount > 0) ? new string[initialCount] : Array.Empty<string>();
                this.name = name;
            }
        }

        // 构造函数可以委派给父类 base() 也可以同类this(), 但不能同时
        class Person
        {
            private string name;
            private int age;

            public Person()
            {
            }
        }

        class Man : Person
        {
            private bool IsMan;
            private bool IsWoman;

            public Man() : base()
            {
            }

            public Man(bool isMan , bool isWoman)
            {
                this.IsMan = isMan;
                this.IsWoman = isWoman;
            }

            public Man(bool isMan) : this(true,false) // base()
            {
            }
            
            // 一般优先考虑默认值，但尽量避免更改给客户端带来影响
            
            // 首个实例系统执行顺序
            // 1. 存放静态变量空间清零
            // 2. 执行静态变量初始化语句
            // 3. 执行基类的静态构造函数
            // 4. 执行本类静态构造函数
            // 
            // 5. 存放实例变量的空间清零
            // 6. 执行实例变量的初始化语句
            // 
            // 7. option 执行基类的实例构造函数
            // 8. 执行本类实例构造函数
            
            // 其后再创建实例，从step5开始
            
        }
    }
}