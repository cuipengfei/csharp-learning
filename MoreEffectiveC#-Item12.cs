using System;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace csharp_learning
{
    // 尽量用可选参数来取代方法重载
    public class MoreEffectiveCSharpItem12
    {
        public class User
        {
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public int Age { get; init; }
            public string Gender { get; init; }
        }

        public static User register(
            string firstName = "",
            string lastName = "",
            int age = 0,
            string gender = "")
        {
            return new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Gender = gender
            };
        }

        [Fact]
        public void Test()
        {
            // 如果不使用可选参数，将必须给每个参数赋值，及时内容为空
            var user1 = register("", "", 0, "");
            
            // 如果使用位置参数，必须给前N位赋值
            var user2 = register("", "", 10);
            
            // 使用命名参数，只需要给特定的参数值(推荐做法)
            var user3 = register(age: 10);
            // 甚至可以调换顺序（但不推荐）
            // 当两个参数的类型相同时，传参时加上参数名，有助于提高代码可读性
            var user4 = register(lastName: "last name", firstName: "first name");
            // 使用可选的命名参数这种方式给予参数值，一个方法即可对应多种参数组合。如果使用重载，要用到几种参数组合，就要写几个方法。
        }
        
        // 开发时，第一次创建方法时，尽量使用可选的命名参数
        // 之后，尽量避免修改参数名。不是breaking change。但使用者下次编译时会报错
        // 用重载的方法增加参数。因为这是breaking change。（跨程序集调用的场景）
    }
}