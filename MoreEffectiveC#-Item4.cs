using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DefaultNamespace;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace csharp_learning_4
{
    /*
     * 注意值类型与引用类型之间的区别
     * 选择的原则：
     *
     * 底层的数据对象最好使用值类型来表示，而应用程序的行为则适合放在引用类型中
     * 在适当的地方使用值类型，可以让你从类对象中安全的导出数据副本，还可以提高内存的使用效率
     * 在适当的地方使用引用类型，可以让你利用标准的面向对象的技术来编写应用程序的逻辑代码
     * 如果你不确定某个类型将来怎么用，那就优先考虑设为引用类型吧！
     */


#pragma warning disable 618
#pragma warning disable CA2000
    public class BankAccount
    {
        public decimal Balance { get; set; }
    }

    [Serializable]
    public struct Employee1
    {
        public string Position { get; set; }
        public decimal CurrentPayAmount { get; set; }

        public void Pay(BankAccount b) => b.Balance += CurrentPayAmount;
    }


    [Serializable]
    public class Employee2
    {
        public string Position { get; set; }
        public decimal CurrentPayAmount { get; set; }

        public void Pay(BankAccount b) => b.Balance += CurrentPayAmount;
    }

    public class Test
    {
        [Fact]
        public void CreateEmployeesTest1()
        {
            var employees1FromStruct = new Employee1[100];
            var employees2FromClass = new Employee2[100];

            var stream1 = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream1, employees1FromStruct);
            stream1.Position = 0;

            var stream2 = new MemoryStream();
            formatter.Serialize(stream2, employees2FromClass);
            stream2.Position = 0;

            Assert.True((stream1.Length - stream2.Length) > 0);
        }


        [Fact]
        public void AddBonusTest1()
        {
            var employees = new Employee1[100];
            employees[0] = new Employee1()
            {
                Position = "CEO",
                CurrentPayAmount = 1000
            };
            var e1 = employees.First(e => e.Position == "CEO");
            var ceoBankAccount = new BankAccount();
            const decimal bonus = 1000;
            e1.CurrentPayAmount += bonus;
            e1.Pay(ceoBankAccount);

            Assert.Equal(2000, e1.CurrentPayAmount);
            Assert.Equal(1000, employees[0].CurrentPayAmount);
        }

        [Fact]
        public void AddBonusTest2()
        {
            var employees = new Employee2[100];
            employees[0] = new Employee2()
            {
                Position = "CEO",
                CurrentPayAmount = 1000
            };
            var e2 = employees.First(e => e.Position == "CEO");
            var ceoBankAccount = new BankAccount();
            const decimal bonus = 1000;
            e2.CurrentPayAmount += bonus;
            e2.Pay(ceoBankAccount);

            Assert.Equal(2000, e2.CurrentPayAmount);
            Assert.Equal(2000, employees[0].CurrentPayAmount);
        }
    }
}