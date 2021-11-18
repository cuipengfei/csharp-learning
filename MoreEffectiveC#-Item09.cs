using System;
using Xunit;

namespace csharp_learning
{
    // 理解相等的不同概念及他们之间的关系
    public class MoreEffectiveC__Item09
    {
        /*
         * 1. public static bool ReferenceEquals(object left, object right)
         * 2. public static bool Equals(object left, object right)
         * 3. public virtual bool Equals(object right)
         * 4. public static bool operator ==(MyClass left, MyClass right)
         *
         * 重写Equals()都要实现IEquatable<T>接口，从值的意义上判断是否相等，还应该实现IStructuralEquatable接口
         */

        // 1. ReferenceEquals 装箱的存在会导致看起来相同的值类型并不相等
        [Fact]
        public void NotEqual()
        {
            int i = 5;
            int j = 5;
            Assert.False(Object.ReferenceEquals(i, j));
            Assert.False(Object.ReferenceEquals(i, i));
        }

        // 2. public static bool Equals(object left, object right)
        public static bool Equals(object left, object right)
        {
            if (Object.ReferenceEquals(left, right))
                return true;
            if ((Object.ReferenceEquals(left, null)) || (Object.ReferenceEquals(right, null)))
                return false;
            return left.Equals(right);
        }

        // 3. public virtual bool Equals(object right)
        /*
         * 默认情况下，Equals()方法判断对象引用是否相同，对于值类型，判断引用加内容，但默认方式通过反射实现，实现效率不高
         * 所以，创建值类型默认需要实现Equals()方法
         * 对于引用类型，如果需要根据其值语义（内容）来判断是否相等，则重写Equals()方法
         * 重写Equals()也要重写GetHashCode()方法
         */

        // 标准做法
        public class Foo : IEquatable<Foo>
        {
            public bool Equals(Foo? other)
            {
                return true;
            }

            public override bool Equals(Object other)
            {
                if (Object.ReferenceEquals(other, null))
                    return false;
                if (Object.ReferenceEquals(this, other))
                    return true;
                // this 和 other 都未必是Foo类型，可能是子类
                if (this.GetType() != other.GetType())
                    return false;
                return this.Equals(other);
            }
        }
        
        [Fact]
        public void Strange()
        {
            ClassA newA = new ClassA();
            ClassB newB = new ClassB();
            Assert.True(newA.Equals(newB));
            Assert.True(newB.Equals(newA));
        }
        
        // 4. public static bool operator ==(MyClass left, MyClass right)
        /*
         * .Net Framework中的类都认为 == 运算符在除string以外的引用类型上应该按照引用判断是否相等
         */
    }

    public class ClassA : IEquatable<ClassA>
    {
        public int A = 1;
        public bool Equals(ClassA? other)
        {
            return other.A == this.A;
        }
        
        public override bool Equals(Object other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            if (Object.ReferenceEquals(this, other))
                return true;
            
            return this.Equals(other);
        }
    }
    
    public class ClassB : ClassA, IEquatable<ClassB>
    {
        public int B = 2;
        public bool Equals(ClassB? other)
        {
            return other.A == this.A && other.B == this.B;
        }
        
        public override bool Equals(Object other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            if (Object.ReferenceEquals(this, other))
                return true;
            
            return this.Equals(other);
        }
    }
}