using System;
using System.Globalization;
using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class ConvertTests
    {
        [Test]
        public void StringToNumber()
        {
            const string number = "1";

            int integer = (int) Convert.ChangeType(number, typeof (int));

            Assert.AreEqual(1, integer);
        }

        [Test]
        public void StringToNumber_TypeCode()
        {
            const string number = "1";

            long longNumber = (long) Convert.ChangeType(number, TypeCode.Int64);

            Assert.AreEqual(1, longNumber);
        }

        [Test]
        public void StringToNumber_TypeCode_FormatProvider()
        {
            const string date = "2015-02-23";

            DateTime convertedDate = (DateTime) Convert.ChangeType(date, TypeCode.DateTime, CultureInfo.CreateSpecificCulture("fr-CH"));

            Assert.AreEqual(2015, convertedDate.Year);
            Assert.AreEqual(2, convertedDate.Month);
            Assert.AreEqual(23, convertedDate.Day);
        }
    }
}
