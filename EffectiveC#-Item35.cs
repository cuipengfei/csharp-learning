using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace csharp_learning
{
    public sealed class PersonOf35
    {
        public string FirstName { get; init; }
        
        public string LastName { get; init; }
    }

    public static class ConsoleReport
    {
        public static string Format(this PersonOf35 person) => $"{person.FirstName} {person.LastName}";
    }

    public static class XmlReport
    {
        // public static string Format(this PersonOf35 person) => new XElement("Person", new XElement("LastName", person.LastName), new XElement("FirstName", person.FirstName)).ToString();
    }

    public class UnitTest35
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest35(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        private void ConsoleOutput()
        {
            var somePresidents = new List<PersonOf35>
            {
                new PersonOf35 {FirstName = "George", LastName = "Washington"},
                new PersonOf35 {FirstName = "Thomas", LastName = "Jefferson"},
                new PersonOf35 {FirstName = "Abe", LastName = "Lincoln"}
            };

            for (int i = 0; i < somePresidents.Count; i++)
            {
                PersonOf35 p = somePresidents[i];
                _testOutputHelper.WriteLine(p.Format());
                Assert.Equal($"{p.FirstName} {p.LastName}", p.Format());
            }
        }
    }
}