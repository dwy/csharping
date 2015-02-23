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
        public void StringToDate_TypeCode_FormatProvider()
        {
            const string date = "2015-02-23";

            DateTime convertedDate = (DateTime) Convert.ChangeType(date, TypeCode.DateTime, DateTimeFormatInfo.InvariantInfo);

            Assert.AreEqual(2015, convertedDate.Year);
            Assert.AreEqual(2, convertedDate.Month);
            Assert.AreEqual(23, convertedDate.Day);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void InvalidFormat_Throws()
        {
            const string date = "invalid date";

            Convert.ChangeType(date, TypeCode.Double);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void InvalidCast_Throws()
        {
            const byte value = 255;

            Convert.ChangeType(value, TypeCode.DateTime);
        }

        [Test]
        public void ToXyzExamples()
        {
            bool boolean = Convert.ToBoolean("true");
            char c = Convert.ToChar(0x32);
            sbyte sByte = Convert.ToSByte(123);

            Assert.AreEqual(true, boolean);
            Assert.AreEqual('2', c);
            Assert.AreEqual(123, sByte);
        }
    }
}
