using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace csharp_learning
{
    public class EffectiveCSharpItem44
    {
        // 44. 不要修改绑定变量。
        
        #region WriteLine
        
        private readonly ITestOutputHelper _testOutputHelper;

        public EffectiveCSharpItem44(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        #endregion
        
        [Fact]
        private void Opening()
        {
            var index = 0;
            Func<IEnumerable<int>> sequence = () => Utilities.Generate(10, () => index++);

            index = 20;
            foreach (var n in sequence())
            {
                _testOutputHelper.WriteLine(n.ToString());
            }
            Assert.Equal(30,sequence().First());

            _testOutputHelper.WriteLine("==========");

            index = 100;
            foreach (var n in sequence())
            {
                _testOutputHelper.WriteLine(n.ToString());
            }
            Assert.Equal(110,sequence().First());
        }
    }

    #region 静态委托
    
    public class StaticDelegate
    {
        private void CSharpCode()
        {
            int[] someNumbers = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var answers = from n in someNumbers
                select n * n;
        }

        // private void ILCode()
        // {
        //     private static int HiddenFunc(int n) => (n * n);
        //
        //     private static Func<int, int> HiddenDelegateDefinition;
        //
        //     int[] someNumbers = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        //     if (HiddenDelegateDefinition == null)
        //     {
        //         HiddenDelegateDefinition = new Func<int, int>(HiddenFunc);
        //     }
        //
        //     var answers = someNumbers.Select<int, int>(HiddenDelegateDefinition);
        // }
    }
    
    #endregion
    
    #region 实例委托

    public class ModFilter
    {
        private readonly int _modulus;

        public ModFilter(int mod)
        {
            _modulus = mod;
        }

        public IEnumerable<int> FindValues(IEnumerable<int> sequence)
        {
            return from n in sequence
                where n % _modulus == 0
                select n * n;
        }
    }

    public class ModFilterIl
    {
        private readonly int modulus;
        
        private bool WhereClause(int n) => n % modulus == 0;
        
        private static int SelectClause(int n) => n * n;
        
        private static Func<int, int> SelectDelegate;

        public IEnumerable<int> FindValues(IEnumerable<int> sequence)
        {
            SelectDelegate ??= SelectClause;

            return sequence.Where(WhereClause).Select(SelectDelegate);
        }
    }
    
    #endregion
    
    #region 闭包

    public class ModFilter2
    {
        private readonly int modulus;

        public ModFilter2(int mod)
        {
            modulus = mod;
        }

        public IEnumerable<int> FindValues(IEnumerable<int> sequence)
        {
            int numValues = 0;
            return from n in sequence
                where n % modulus == 0
                select n * n / ++numValues;
        }
        
        // Other methods elided.
    }

    // public class ModFilter2Il
    // {
    //     private sealed class Closure
    //     {
    //         public ModFilter2 outer;
    //         public int numValues;
    //
    //         public int SelectClause(int n) => n * n / numValues;
    //     }
    //
    //     private readonly int modulus;
    //
    //     public ModFilter2IL(int mod)
    //     {
    //         modulus = mod;
    //     }
    //
    //     private bool WhereClause(int n) => n * modulus == 0;
    //
    //     public IEnumerable<int> FindValues(IEnumerable<int> sequence)
    //     {
    //         var c = new Closure();
    //         c.outer = this;
    //         c.numValues = 0;
    //         return sequence.Where(WhereClause).Select(c.SelectClause);
    //     }
    // }
    
    #endregion
    
    #region 工具类
    
    public static class Utilities
    {
        public static IEnumerable<int> Generate(int count, Func<int> func)
        {
            var numbers = new List<int>();

            for (var i = 0; i < count; i++)
            {
                numbers.Add(func());
            }

            return numbers;
        }
    }
    
    #endregion
}