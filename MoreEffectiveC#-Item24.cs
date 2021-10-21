using System;
using Xunit;

namespace csharp_learning
{
    // 避免实现ICloneable接口
    public class MoreEffectiveCSharpItem24
    {
        // 原因： 会产生较大的限制
        // 限制一： 派生类必须也实现ICloneable接口
        public class BaseType : ICloneable
        {
            private string label = "class name";
            private int[] values = new int[10];

            public object Clone()
            {
                var res = new BaseType();
                res.label = label;
                for (int i = 0; i < values.Length; i++)
                {
                    res.values[i] = values[i];
                }

                return res;
            }
        }

        public class DerivedType : BaseType
        {
            private double[] dValues = new double[10];
        }

        [Fact]
        public void ShouldForceDerivedTypeToImplementICloneableAsWell()
        {
            var origin = new DerivedType();
            var copied = origin.Clone();
            
            Assert.Null(copied as DerivedType);
            Assert.Equal(typeof(BaseType), copied.GetType());
            Assert.NotEqual(typeof(DerivedType), copied.GetType());
        }


        // 限制二：成员变量也要支持ICloneable接口
        public class Door: ICloneable
        {
            public string Material { get; set; }
            public object Clone()
            {
                return new Door {Material = Material};
            }
        }

        public class Window: ICloneable
        {
            public double Area { get; set; }
            public object Clone()
            {
                return new Window {Area = Area};
            }
        }
        
        public class Room: ICloneable
        {
            public Door Door { get; set; }
            public Window Window { get; set; }
            
            public object Clone()
            {
                return new Room
                {
                    Door = Door.Clone() as Door,
                    Window = Window.Clone() as Window
                };
            }
        }

        [Fact]
        public void MemberFieldsShouldAlsoSupportICloneale()
        {
            var origin = new Room
            {
                Door = new Door {Material = "Wood"},
                Window = new Window {Area = 1}
            };
            var copied = origin.Clone() as Room;

            origin.Door.Material = "Iron";
            origin.Window.Area = 2;
            
            Assert.Equal("Wood", copied.Door.Material);
            Assert.Equal(1, copied.Window.Area);
        }
        
        // 如果必须要实现，推荐做法
        class Base
        {
            private string label;
            private int[] values;

            protected Base()
            {
                label = "class name";
                values = new int[10];
            }

            // 不实现ICloneable，而是提供一个protected copy constructor,让派生类可以复制基类部分的字段
            protected Base(Base right)
            {
                label = right.label;
                values = right.values.Clone() as int[];
            }
        }

        // 派生类是sealed，不要让层级继续加深，复制链变得更复杂
        // 派生类可以不实现ICloneable接口
        sealed class Derived : Base, ICloneable
        {
            private double[] dValues;

            public Derived()
            {
                dValues = new double[10];
            }

            public Derived(Derived right): base(right)
            {
                dValues = right.dValues.Clone() as double[];
            }
            
            public object Clone()
            {
                return new Derived(this);
            }
        } 
    }
}