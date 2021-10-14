using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_learning
{
    // Item 28
    // 考虑通过扩展方法，增强已构造类型的功能
    // 使用场景： 集合中存入的元素类型比较特殊，需要这对这种特殊元素的集合提供一些特殊的功能
    // 好处： 将数据的存储模型和使用方式解耦

    public class Customer
    {
        public string Name { get; set; }
        public string EMailAddress { get; set; }

        public bool Verify()
        {
            Console.Out.WriteLine("Verify custom information");
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(EMailAddress);
        }

        public void SendEmail(string content)
        {
            Console.Out.Write($"Send email to {Name}({EMailAddress}): {content}");
        }
    }

    public static class CustomExtension
    {
        public static void Register(this ICollection<Customer> customers, Customer newCustomer)
        {
            if (newCustomer.Verify())
            {
                newCustomer.SendEmail("Welcome!");
                customers.Add(newCustomer);
            }
            else
            {
                throw new Exception("Verify custom information failed");
            }
        }

        public static void SendCoupons(this ICollection<Customer> customers)
        {
            customers.ForAll(customer => customer.SendEmail("You receiverd a coupon luckly!"));
        }

        public static Customer FindByName(this ICollection<Customer> customers, string name)
        {
            return customers.First(customer => customer.Name.Equals(name));
        }

        public static void SendEmailTo(this ICollection<Customer> customers, string name, string content)
        {
            customers.FindByName(name).SendEmail(content);
        }
    }

    // 也可以将特殊元素集合定义成一个类，但需要继承一个特定的集合类
    public class CustomerCollection : List<Customer>
    {
        public Customer FindByName(string name)
        {
            return this.First(customer => customer.Name.Equals(name));
        }
        // Register， SendCoupons，SendEmailTo
    }

    public class EffectiveCItem28
    {
        [Fact]
        public void CanSupportMultipleStorageFormats()
        {
            ICollection<Customer> customerSet = new HashSet<Customer>();
            customerSet.Register(new Customer {Name = "Tom", EMailAddress = "Tom@email.com"});
            customerSet.Register(new Customer {Name = "John", EMailAddress = "John@email.com"});
            customerSet.SendCoupons();
            customerSet.SendEmailTo("Tom", "Happy Birthday");

            ICollection<Customer> customerList = new List<Customer>();
            customerList.Register(new Customer {Name = "Tom", EMailAddress = "Tom@email.com"});
            customerList.Register(new Customer {Name = "John", EMailAddress = "John@email.com"});
            customerList.SendCoupons();
            customerList.SendEmailTo("Tom", "Happy Birthday");
        }
    }
}