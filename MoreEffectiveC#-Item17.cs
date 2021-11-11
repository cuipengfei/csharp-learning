using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Xunit;

/*
 * 不要把类的内部对象通过引用返回给外界*
*/

namespace Xunit16
{
    public class MyBusinessObject1
    {
        public MyBusinessObject1()
        {
            ListOfData = new Collection<ImportantData> {new ImportantData()};
        }

        public Collection<ImportantData> ListOfData { get; }
    }

    public class MyBusinessObject2
    {
        private readonly Collection<ImportantData> _listOfData = new Collection<ImportantData> {new ImportantData()};

        // 返回给调用方的对象声明为接口类型，尽量防止被"无端"修改
        public IEnumerable RestrictedCollectionOfData =>
            _listOfData;

        // 用包装器实现只读的数据视图
        public ReadOnlyCollection<ImportantData> ReadOnlyCollectionOfData =>
            new(_listOfData);

        //类似的：ReadOnlyDictionary
    }


    public class ImportantData
    {
    }

    public class Test16
    {
        [Fact]
        public void UnExpectedTest()
        {
            var bizObj = new MyBusinessObject1();

            // Access the collection
            var stuff = bizObj.ListOfData;
            Assert.Single(bizObj.ListOfData);

            // Not intended, but allowed
            stuff.Clear(); // Deletes all data
            Assert.Empty(bizObj.ListOfData);
        }

        [Fact]
        public void UsingInterfaceToExposeInnerDataTest()
        {
            var bizObj = new MyBusinessObject2();

            var stuff = bizObj.RestrictedCollectionOfData;
            Assert.Single(stuff);

            // not allowed
            // stuff.Clear();

            // but still allow
            ((ICollection<ImportantData>) stuff).Clear();
            Assert.Empty(bizObj.RestrictedCollectionOfData);
        }

        [Fact]
        public void UsingReadonlyWrapperToExposeInnerDataTest()
        {
            var bizObj = new MyBusinessObject2();

            var stuff = bizObj.ReadOnlyCollectionOfData;
            Assert.Single(stuff);


            // not allowed, compiler error
            // stuff.Clear();

            // not allowed, runtime error, not supported exception:collection is read-only
            // ((ICollection<ImportantData>)stuff).Clear();
        }
    }
}