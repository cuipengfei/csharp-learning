using System;
using Xunit;

namespace csharp_learning.Item2
{

    public class UnitTest2
    {
        // Compile-time constant
        public const int _year = 2000;
        // Runtime constant
        public readonly int _year2 = 2000;
        
        // diff support--const 只支持数字、字符串、null
        //private const DateTime _dataTime1 = new DateTime();
        private readonly DateTime _dataTime2 = new DateTime();

        public UnitTest2(int year)
        {
            // readonly 可以在构造函数中重新赋值一次，const不可以
            //_year = year;
            _year2 = year;
        }

        private void createConstant(){
            const int year3 = 2000;
            int myYear1 = year3; // myYear1 = 2000;
            // readonly 不可以在方法体内部声明
            //readonly int year4 = 3000;
        }
        // const编译时写入，更改后需要重新编译
        // readonly运行时写入，更改后无需重新编译
        
        // const性能更好
        // const用来声明必须编译期确定的值，除此之外的应该考虑readonly
        
    }
}
