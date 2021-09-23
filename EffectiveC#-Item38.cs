using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DefaultNamespace
{
    // 考虑使用lambda表达式来代替方法
    public class UnitTest38
    {
        [Fact]
        public void VersionWithoutRefactor()
        {
            var allEmployees = new Employees().FindAllEmployees();
            
            //Find the first employee
            var earlyFolks = from e in allEmployees
                where e.Classification == EmployeeType.Salary
                where e.YearsOfService > 20
                where e.MonthlySalary < 4000
                select e;
            
            // Find the newest people
            var newest = from it in allEmployees
                where it.Classification == EmployeeType.Salary
                where it.YearsOfService < 20
                where it.MonthlySalary < 4000
                select it;
        }
        
        
        [Fact]
        public void Version2RefactorWithExtractMethod()
        {
            var allEmployees = new Employees().FindAllEmployees();
            
            //Find the first employee
            var earlyFolks = from e in allEmployees
                where LowPaidSalaried(e)
                where e.YearsOfService > 20
                select e;

            // Find the newest people
            var newest = from it in allEmployees
                where LowPaidSalaried(it)
                where it.YearsOfService < 20
                select it;
        }
        
        [Fact]
        public void Version3RefactorWithExtractSmallLambda()
        {
            var allEmployees = new Employees().FindAllEmployees();
            
            //Find the first employee
            var salaried = allEmployees.LowPaidSalariedFilter();


            var earlyFolks = salaried.Where(e => e.YearsOfService > 20);
            
            
            // Find the newest people
            var newest = salaried.Where(e => e.YearsOfService < 2);;
        }

        private static bool LowPaidSalaried(Employees e) =>
            e.MonthlySalary < 4000 && e.Classification == EmployeeType.Salary;
    }

    public static class ExtensionMethod
    {
        public static IEnumerable<Employees> LowPaidSalariedFilter(this IEnumerable<Employees> sequence) =>
            from s in sequence
            where s.Classification == EmployeeType.Salary && s.MonthlySalary < 4000
            select s;
    }


    public class Employees
    {
        public EmployeeType Classification;
        public int YearsOfService;
        public double MonthlySalary;

        public Employees()
        {
        }

        public Employees(EmployeeType classification, int yearsOfService, double monthlySalary)
        {
            Classification = classification;
            YearsOfService = yearsOfService;
            MonthlySalary = monthlySalary;
        }

        public IEnumerable<Employees> FindAllEmployees()
        { 
            var result = new List<Employees>()
            {
                new Employees(EmployeeType.Salary, 25, 5000),
                new Employees(EmployeeType.Salary, 1, 3000),
                new Employees(EmployeeType.Salary, 5, 3500),
                new Employees(EmployeeType.Salary, 15, 8000)
            };

            return from e in result
                select e;
        }
    }

    public enum EmployeeType
    {
        Salary
    }
}