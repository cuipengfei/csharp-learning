using System;
using Xunit;

namespace csharp_learning
{


    public class UnitTest5
    {

        [Fact]
        public void VarShouldNotLoseType()
        {
            DateTime dateTime = DateTime.Parse("2021-01-01");

            String s =
                $"Date {dateTime}"; //s的内容取决于执行这行代码的机器的设置

            FormattableString fs =
                $"Date {dateTime}";  //fs的内容则不是死的，可以在调整为任何文化              

            String gs = ToGerman(fs);
        
            Assert.Equal(gs, "Date 01.01.2021 00:00:00");
        }

        public static string ToGerman(FormattableString src)
        {
            return string.Format(                
                System.Globalization.CultureInfo.CreateSpecificCulture("de-de"),
                src.Format,
                src.GetArguments());
        }

    }
}