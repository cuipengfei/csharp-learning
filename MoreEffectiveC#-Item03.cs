using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace DefaultNamespace
{
    public class MoreEffectiveC__Item03
    {
        // 对于值类型，尽量设计成不可变的类型

        public struct Address
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public int ZipCode { get; set; }
        }

        private IDictionary<string, int> validAddress = new Dictionary<string, int>()
        {
            {"AnyTown",666666},{"Ann Arbor",888888}
        };
        private bool IsValidAddress( Address address)
        {
            return address.ZipCode.Equals(validAddress[address.City]);

        }

        [Fact]

        public void ShouldBeTrueIfAddressIsValid()
        {
            var address = new Address()
            {
                Line1 = "111S.Main",
                Line2 = "222s.Main",
                City = "AnyTown",
                State = "IL",
                ZipCode = 666666
            };
            Assert.True(IsValidAddress(address));
        }

        [Fact]
        public void ShouldChangeValueOfAttribute()
        {
            var address = new Address()
            {
                Line1 = "111S.Main",
                Line2 = "222s.Main",
                City = "AnyTown",
                State = "IL",
                ZipCode = 666666
            };
            //  如果我们可以对于已创建出来的实例进行修改，就可能会破坏对象所要求的不变关系
            address.City = "Ann Arbor";
            Assert.False(IsValidAddress(address));
        }

        public struct BetterAddress
        {
            public BetterAddress(string line1,string line2,string city,string state, int zipCode)
            {
                Line1 = line1;
                Line2 = line2;
                City = city;
                State = state;
                ZipCode = zipCode;
            }

            public string Line1 { get; }
            public string Line2 { get; }
            public string City { get; }
            public string State { get; }
            public int ZipCode { get; }
        }

        private void CallAddress()
        {
            BetterAddress firstAddress = new BetterAddress("111S.Main", "222s.Main", "AnyTown", "IL", 666666);
            
            // To chang re-initialize
            BetterAddress secondAddress = new BetterAddress("111S.Main", "222s.Main", "Ann Arbor", "IL", 88888888);
        }
        
        public class Phone
        {
            public int number { get; set; }
        }
        
        public struct PhoneList
        {
            public PhoneList(Phone[] phones)
            {
                Phones = phones;
            }

            public Phone[] Phones { get; }
        }

        [Fact]
        public void ShouldChangeContent()
        {
            var phones = new Phone[]
            {
                new()
                {
                    number = 1234567890
                },
                new()
                {
                    number = 0987654321
                }
            };
            PhoneList phoneList = new PhoneList(phones);
            Assert.Equal(1234567890,phoneList.Phones.First().number);
            phones[0] = new Phone()
            {
                number = 1234567891
            };
            Assert.Equal(1234567891,phoneList.Phones.First().number);
        }

        #region SecondPhoneList
        public struct SecondPhoneList
        {
            public SecondPhoneList(ImmutableList<Phone> phones)
            {
                Phones = phones;
            }

            public IEnumerable<Phone> Phones { get; }
        }
        
        [Fact]
        public void ShouldNotChangeSecondPhoneListContent()
        {
            var phones = ImmutableList<Phone>.Empty;
            var newPhones = phones.Add(new Phone
            {
                number = 1234567890
            }).ToImmutableList();
            SecondPhoneList secondPhoneList = new SecondPhoneList(newPhones);
            var changeImmutablePhones = ChangeImmutablePhones(newPhones);
            Assert.Equal(1,newPhones.Count());
            Assert.Equal(1234567890,newPhones.First().number);
            Assert.Equal(1111111111,changeImmutablePhones.First().number);
            Assert.Equal(1234567890,secondPhoneList.Phones.First().number);
        }

        private ImmutableList<Phone> ChangeImmutablePhones(ImmutableList<Phone> phones)
        {
            try
            {
                var phone = phones[0];
                var immutableList = phones.Remove(phone).Add(new Phone()
                {
                    number = 1111111111
                }).ToImmutableList();
                return immutableList;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

    }
}