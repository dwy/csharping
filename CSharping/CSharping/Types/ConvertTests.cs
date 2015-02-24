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

        [Test]
        public void CustomConvertibleClass()
        {
            var value = new CustomConvertibleValue(42);

            byte b = Convert.ToByte(value);
            char c = Convert.ToChar(value);
            decimal d = Convert.ToDecimal(value);

            Assert.AreEqual(42, b);
            Assert.AreEqual('*', c);
            Assert.AreEqual(42m, d);
            Assert.Throws<InvalidCastException>(() => Convert.ToDateTime(value));
        }

        class CustomConvertibleValue : IConvertible
        {
            private readonly int _value;

            public CustomConvertibleValue(int value)
            {
                _value = value;
            }

            public TypeCode GetTypeCode()
            {
                return TypeCode.Object;
            }

            public bool ToBoolean(IFormatProvider provider)
            {
                return _value > 0;
            }

            public char ToChar(IFormatProvider provider)
            {
                return (char) _value;
            }

            public sbyte ToSByte(IFormatProvider provider)
            {
                return (sbyte) _value;
            }

            public byte ToByte(IFormatProvider provider)
            {
                return (byte) _value;
            }

            public short ToInt16(IFormatProvider provider)
            {
                return (Int16) _value;
            }

            public ushort ToUInt16(IFormatProvider provider)
            {
                return (ushort) _value;
            }

            public int ToInt32(IFormatProvider provider)
            {
                return _value;
            }

            public uint ToUInt32(IFormatProvider provider)
            {
                return (uint) _value;
            }

            public long ToInt64(IFormatProvider provider)
            {
                return (long) _value;
            }

            public ulong ToUInt64(IFormatProvider provider)
            {
                return (ulong) _value;
            }

            public float ToSingle(IFormatProvider provider)
            {
                return (float) _value;
            }

            public double ToDouble(IFormatProvider provider)
            {
                return (double) _value;
            }

            public decimal ToDecimal(IFormatProvider provider)
            {
                return (decimal) _value;
            }

            public DateTime ToDateTime(IFormatProvider provider)
            {
                throw new InvalidCastException();
            }

            public string ToString(IFormatProvider provider)
            {
                return _value.ToString(provider);
            }

            public object ToType(Type conversionType, IFormatProvider provider)
            {
                throw new NotImplementedException();
            }
        }
    }
}
