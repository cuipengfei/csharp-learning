using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace csharp_learning
{
    public struct MyStruct
    {
        public string msg { get; private set; }
        public int id { get; private set; }
        public DateTime epoch { get; private set; }

        public MyStruct(string msg, int id, DateTime epoch)
        {
            this.msg = msg;
            this.id = id;
            this.epoch = epoch;
        }
    }

    //do nothing. 
    //All reference types will have a hash code that is correct, even if it is very inefficient
    public class MyPerson
    {
        public MyPerson(string name) => this.Name = name;

        public string Name { get; set; }

        public override int GetHashCode() => Name.GetHashCode(); //bad example
    }

    public class MoreEffectiveCSharpItem10
    {
        [Fact]
        public void BadExampleOfObjHashCode()
        {
            var dict = new Dictionary<MyPerson, String>();

            var abc = new MyPerson("ABC");
            dict.Add(abc, "hello");

            Assert.Equal("hello", dict.GetValueOrDefault(abc, "nothing"));


            abc.Name = "hi";
            Assert.Equal("nothing", dict.GetValueOrDefault(abc, "nothing"));//找不到了
        }

        [Fact]
        public void ExampleOfValueTypeHashCode()
        {
            var myStruct = new MyStruct("hello", 123, new DateTime());

            //returns the hash code from the first field defined in the type
            //这里书上讲的内容与验证出来的不符合
            //https://stackoverflow.com/questions/47009543/how-is-base-gethashcode-implemented-for-a-struct
            Assert.NotEqual(myStruct.GetHashCode(), myStruct.msg.GetHashCode());
        }
    }
}