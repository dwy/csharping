using System;
using System.Security.Policy;
using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/cc138362.aspx

    [TestFixture]
    public class EnumTests
    {
        [Test]
        public void EnumsAreValueTypes()
        {
            const Days day = Days.Friday;

            Assert.IsInstanceOf<ValueType>(day);
        }

        [Test]
        public void DefaultUnderlyingTypeIsInt_StartsAtZero_CountsUp()
        {
            const int sundayNumber = (int) Days.Sunday;
            const int mondayNumber = (int) Days.Monday;
            const int saturdayNumber = (int) Days.Saturday;

            Assert.AreEqual(0, sundayNumber);
            Assert.AreEqual(1, mondayNumber);
            Assert.AreEqual(6, saturdayNumber);
        }

        enum Days
        {
            Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
        }

        [Test]
        public void SpecifyMemberValue()
        {
            const int noneValue = (int) AddressType.None;
            const int shipmentValue = (int) AddressType.Shipment;
            const int billValue = (int) AddressType.Bill;

            Assert.AreEqual(0, noneValue);
            Assert.AreEqual(3, shipmentValue);
            Assert.AreEqual(4, billValue);
        }

        enum AddressType
        {
            None = 0, Payment = 2, Shipment, Bill
        }

        [Test]
        public void SpecifyUnderlyingType()
        {
            const byte codeMonkeyValue = (byte)EmployeeType.CodeMonkey;

            Assert.AreEqual(128, codeMonkeyValue);
        }

        enum EmployeeType : byte
        {
            CodeMonkey = 128, Manager = 255
        }

        [Test]
        public void Flags_CombineValues_BitwiseOr()
        {
            const CoffeeOptions longOption = CoffeeOptions.Long;
            const CoffeeOptions milkOption = CoffeeOptions.Milk;
            // 0110 = 0010 + 0100
            const CoffeeOptions options = CoffeeOptions.Long | CoffeeOptions.Milk;

            Assert.AreEqual(2, (int)longOption);
            Assert.AreEqual(4, (int)milkOption);
            Assert.AreEqual(6, (int)options);

            Assert.AreEqual((int)options, (int)longOption + (int)milkOption);
        }

        [Test]
        public void Flags_CheckPresenceOfFlag_BitwiseAnd()
        {
            const CoffeeOptions options = CoffeeOptions.Short | CoffeeOptions.Sugar | CoffeeOptions.Milk;

            const bool isShort = (options & CoffeeOptions.Short) == CoffeeOptions.Short;
            const bool isLong = (options & CoffeeOptions.Long) == CoffeeOptions.Long;
            const bool hasMilk = (options & CoffeeOptions.Milk) == CoffeeOptions.Milk;

            Assert.IsTrue(isShort);
            Assert.IsFalse(isLong);
            Assert.IsTrue(hasMilk);
        }



        [Flags]
        enum CoffeeOptions
        {
            None = 0x0,     // each value should have a different bit set
            Short = 0x01,
            Long = 0x02,
            Milk = 0x04,
            Sugar = 0x08
        }
    }
}
