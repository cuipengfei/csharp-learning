using System.Collections.Generic;

namespace DefaultNamespace
{
    // 考虑使用lambda表达式来代替方法
    public class UnitTest38
    {
        
    }


    public class Employees
    {
        public EmployeeType Classification;
        public int YearsOfService;
        public double MonthlySalary;

        public Employees(EmployeeType classification, int yearsOfService, double monthlySalary)
        {
            Classification = classification;
            YearsOfService = yearsOfService;
            MonthlySalary = monthlySalary;
        }

        public List<Employees> FindAllEmployees()
        {
            return new List<Employees>()
            {
                new Employees(EmployeeType.Salary, 25, 5000),
                new Employees(EmployeeType.Salary, 1, 3000),
                new Employees(EmployeeType.Salary, 5, 3500),
                new Employees(EmployeeType.Salary, 15, 8000)
            };
        }
    }

    public enum EmployeeType
    {
        Salary
    }
}