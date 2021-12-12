using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    public class EffectiveC__Item42
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public EffectiveC__Item42(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public class dbContext : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            }
            public  DbSet<Customer> Customer { get; set; }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string City { get; set; }
            public string Name { get; set; }
        }
        
        
        // IQueryabe and IEnumable
        public void QueryableUse()
        {
            var dbContext = new dbContext();
            var q =

                from c in dbContext.Customer
                where c.City.Equals("Xi'an")
                select c;
            var finalAnswer =
                from c in q
                orderby c.Name
                select c;
        }
        
        public void EnumableUse()
        {
            var dbContext = new dbContext();
            var q =

                (from c in dbContext.Customer
                where c.City.Equals("Xi'an")
                select c).AsEnumerable();
            var finalAnswer =
                from c in q
                orderby c.Name
                select c;
        }

        private static bool isValidCustomer(Customer customer) => customer.Name.LastIndexOf("c", StringComparison.Ordinal).Equals(0);

        public void FindDifference()
        {
            var dbContext = new dbContext();
            // works
            var query =
                from c in dbContext.Customer.AsEnumerable()
                where isValidCustomer(c)
                select c;
            // throw
            var anotherQuery =
                from c in dbContext.Customer
                where isValidCustomer(c)
                select c;
        }

        public static IEnumerable<Customer> ValidCustomer( IEnumerable<Customer> customers) =>
            from customer in customers
            where customer.Name.LastIndexOf("c", StringComparison.Ordinal).Equals(0)
            select customer;
        
        public static IQueryable<Customer> AnotherValidCustomer( IQueryable<Customer> customers) =>
            from customer in customers
            where customer.Name.LastIndexOf("c", StringComparison.Ordinal).Equals(0)
            select customer;
        
        public static IEnumerable<Customer> ThirdValidCustomer( IEnumerable<Customer> customers) => 
            from customer in customers.AsQueryable()
            where customer.Name.LastIndexOf("c", StringComparison.Ordinal).Equals(0)
            select customer;
        
        [Fact]
        public void ShouldGetTiamSpanForQuery()
        {
            var cccc = new Customer();
            var dddd = new Customer();
            _testOutputHelper.WriteLine(nameof(Customer));
            _testOutputHelper.WriteLine(typeof(Customer).ToString());
            _testOutputHelper.WriteLine(cccc.GetType().ToString());
            _testOutputHelper.WriteLine(cccc.GetType().Equals(typeof(Customer)).ToString());
        }
    }
}