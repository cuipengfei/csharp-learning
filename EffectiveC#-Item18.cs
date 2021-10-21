using System;
using System.Collections.Generic;
using Xunit;

namespace csharp_learning
{
    // 只定义刚好够用的约束条件
    public class EffectiveC__Item18
    {
        // 理解类型约束
        public class NoConstraintClass<T>
        {
        }
        
        public class StructConstraint<T> where T : struct //类型参数必须是不可为 null 的值类型。
        {
            public StructConstraint(T value)
            {
            }
        }

        public class ClassConstraint<T> where T : class //类型参数必须是引用类型
        {
            public T Value { get; }

            public ClassConstraint(T value)
            {
                Value = value;
            }
        }
        
        public class NotNullConstraint<T> where T : notnull //类型参数必须是不可为 null 的类型。
        {
            public T Value { get; }

            public NotNullConstraint(T value)
            {
                Value = value;
            }
        }
        
        public class NewConstraint<T> where T : new() //类型参数必须具有公共无参数构造函数
        {
        }
        
        public class BaseClassConstraint<T> where T : People //类型参数必须是指定的基类或派生自指定的基类
        {
            public T Value { get; }

            public BaseClassConstraint(T value)
            {
                Value = value;
            }
        }

        public class InterfaceConstraint<T, U> where T: IComparable where U : People //类型参数必须是指定的接口或实现指定的接口。
        {
        }
        
        public class People
        {
            public string Name { get; set; }
        }
        
        public class Student : People
        {
            public string Grade { get; set; }
        }
        
        [Fact]
        public void SampleTestForLearningConstraint()
        {
            var noConstraintClass = new NoConstraintClass<int>();
            var structConstraint = new StructConstraint<int>(123);
            var classConstraint = new ClassConstraint<string>("123");
            var notnullConstraint = new NotNullConstraint<string>("");
            var newConstraint = new NewConstraint<People>();
            var baseClassConstraint = new BaseClassConstraint<Student>(new Student() { Name = "Vim"});
            var interfaceClassConstraint = new InterfaceConstraint<int, People>();
            
            
            Assert.Equal("123",classConstraint.Value);
            Assert.Equal("",notnullConstraint.Value);
            Assert.Equal("Vim",baseClassConstraint.Value.Name);
        }
        
        // 类型约束可以帮助我们告诉使用者类型参数必须满足的条件；
        // 条件也不能太严格，否则需要开发者做很多的额外工作，
        // 也不能不加约束，那程序需要执行太多的类型转换的工作；
        
        public class CompareDifferentBetweenWithoutConstraintAndWithConstraint
        {
            public bool AreEqual<T>(T left, T right)
            {
                if (left == null)
                {
                    return right == null;
                }

                if (left is IComparable<T> leftValue)
                {
                    if (right is IComparable<T>)
                    {
                        return leftValue.CompareTo(right) == 0;
                    }
                    else
                    {
                        throw new ArgumentException("type does not implement IComparable<T>", nameof(right));
                    }
                }
                else
                {
                    throw new ArgumentException("type does not implement IComparable<T>", nameof(left));
                }
            }

            public bool AreEqualWithConstraint<T>(T left, T right) where T : IComparable<T>
                => left.CompareTo(right) == 0;
        }

        [Fact]
        public void TestForCompareDifferentBetweenWithoutConstraintAndWithConstraint()
        {
            var testAction = new CompareDifferentBetweenWithoutConstraintAndWithConstraint();
            Assert.True(testAction.AreEqual<int>(1,1));
            Assert.True(testAction.AreEqual<People>(null, null));
            Assert.Throws<ArgumentException>(
                () => testAction.AreEqual<List<string>>(new List<string>(), new List<string>()));
            
            Assert.False(testAction.AreEqualWithConstraint<Temperature>(new Temperature(), null));
            Assert.True(testAction.AreEqualWithConstraint<int>(123, 123));
        }
        
        public class Temperature : IComparable<Temperature>
        {
            public int CompareTo(Temperature? other)
            {
                if (other == null) return 1;
                throw new NotImplementedException();
            }
        }
    }
}