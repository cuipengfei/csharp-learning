using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace csharp_learning
{
    public class MoreEffectiveCSharpItem13
    {
        /*
         * 13. 尽量缩减类型的可见范围
         *
         * C# 访问修饰符：
         * 
         * public               任何公有成员可以被外部的类访问。
         * private              只有同一个类中的函数可以访问它的私有成员。
         * protected            该类内部和继承类中可以访问。
         * internal             同一个程序集的对象可以访问。
         * protected internal   符合 protected 或 internal 的任意一条都可以访问。
         *
         * 默认访问修饰符：
         * 
         * enum, interface: public
         * class, struct: private
         */
        
        public MoreEffectiveCSharpItem13()
        {
            var list = new List<string>();
            list.GetEnumerator();

            var dic = new Dictionary<int, string>();
            dic.GetEnumerator();

            var queue = new Queue<string>();
            queue.GetEnumerator();
        }

        private static IPhoneNumberValidator CreateValidator(PhoneNumberTypes phoneNumberType)
        {
            return phoneNumberType switch
            {
                PhoneNumberTypes.UnitedStates => new UsPhoneNumberNumberValidator(),
                PhoneNumberTypes.China => new CnPhoneNumberNumberValidator(),
                _ => new InternationalPhoneNumberNumberValidator()
            };
        }

        [Fact]
        public static void ValidNumber()
        {
            var cnPhoneNumberValidator = CreateValidator(PhoneNumberTypes.China);
            var isValidCnPhoneNumber = cnPhoneNumberValidator.ValidateNumber("18333333333");
            Assert.True(isValidCnPhoneNumber);
        }
    }

    public class UsPhoneNumberNumberValidator : IPhoneNumberValidator
    {
        private static bool IsValidUsPhoneNumber(string usPhoneNumber)
        {
            return Regex.IsMatch(usPhoneNumber, @"/^(\+?1)?[2-9]\d{2}[2-9](?!11)\d{6}$/");
        }

        public bool ValidateNumber(string phoneNumber)
        {
            return IsValidUsPhoneNumber(phoneNumber);
        }
    }

    public class CnPhoneNumberNumberValidator : IPhoneNumberValidator
    {
        private static bool IsValidCnPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"0?(13|14|15|18|17|16|19)[0-9]{9}");
        }
        
        public bool ValidateNumber(string phoneNumber)
        {
            return IsValidCnPhoneNumber(phoneNumber);
        }
    }

    public class InternationalPhoneNumberNumberValidator : IPhoneNumberValidator
    {
        private static bool IsValidInternationalPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "Here is international phone number's pattern.");
        }

        public bool ValidateNumber(string phoneNumber)
        {
            return IsValidInternationalPhoneNumber(phoneNumber);
        }
    }

    public interface IPhoneNumberValidator
    {
        bool ValidateNumber(string phoneNumber);
    }

    public enum PhoneNumberTypes
    {
        UnitedStates,
        China
    }
}