using System;
using System.Linq;
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

        [Test]
        public void DefaultUnderlyingTypeIsInt()
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof (Days));

            Assert.AreEqual(typeof (int), underlyingType);
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

        [Test]
        public void EnumerateValues()
        {
            Array values = Enum.GetValues(typeof (AddressType));
            var intValues = values.Cast<int>().ToList();

            Assert.AreEqual(0, intValues[0]);
            Assert.AreEqual(2, intValues[1]);
            Assert.AreEqual(3, intValues[2]);
            Assert.AreEqual(4, intValues[3]);
        }

        [Test]
        public void EnumerateNames()
        {
            string[] names = Enum.GetNames(typeof (AddressType));

            Assert.AreEqual("None", names[0]);
            Assert.AreEqual("Payment", names[1]);
            Assert.AreEqual("Shipment", names[2]);
            Assert.AreEqual("Bill", names[3]);
        }

        [Test]
        public void GetNameOfAValue()
        {
            const AddressType type = AddressType.Payment;

            string name = Enum.GetName(typeof (AddressType), type);

            Assert.AreEqual("Payment", name);
        }

        [Test]
        public void Parse_ValidString_ReturnsEnumValue()
        {
            var billType = (AddressType)Enum.Parse(typeof (AddressType), "Bill");

            Assert.AreEqual(AddressType.Bill, billType);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void Parse_InvalidString_Throws()
        {
            var billType = (AddressType)Enum.Parse(typeof(AddressType), "Invalid value");
        }

        [Test]
        public void TryParse_ValidString_ReturnsTrue_EnumValueRetrieved()
        {
            const bool ignoreCase = false;
            AddressType result;

            bool valid = Enum.TryParse("Bill", ignoreCase, out result);

            Assert.AreEqual(AddressType.Bill, result);
            Assert.IsTrue(valid);
        }

        [Test]
        public void TryParse_InvalidString_ReturnsFalse_DefaultEnumValueRetrieved()
        {
            const bool ignoreCase = false;
            AddressType result;

            bool valid = Enum.TryParse("Invalid value", ignoreCase, out result);

            Assert.AreEqual(AddressType.None, result); // None is default because it is equal zero
            Assert.IsFalse(valid);
        }

        enum AddressType
        {
            None = 0, Payment = 2, Shipment, Bill
        }

        [Test]
        public void SpecifyUnderlyingType()
        {
            const byte codeMonkeyValue = (byte)EmployeeType.CodeMonkey;

            Type underlyingType = Enum.GetUnderlyingType(typeof (EmployeeType));

            Assert.AreEqual(128, codeMonkeyValue);
            Assert.AreEqual(typeof(byte), underlyingType);
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

        [Test]
        public void Flags_RemoveFlag_BitwiseXor()
        {
            CoffeeOptions options = CoffeeOptions.Short | CoffeeOptions.Sugar | CoffeeOptions.Milk;

            options = options ^ CoffeeOptions.Sugar;

            bool hasSugar = (options & CoffeeOptions.Sugar) == CoffeeOptions.Sugar;
            Assert.IsFalse(hasSugar);
        }

        [Test]
        public void ToObject_SingleValue()
        {
            CoffeeOptions enumValue = (CoffeeOptions)Enum.ToObject(typeof (CoffeeOptions), 0x04);

            Assert.AreEqual(CoffeeOptions.Milk, enumValue);
        }

        [Test]
        public void ToObject_CombinedFlags()
        {
            const CoffeeOptions expected = CoffeeOptions.Milk | CoffeeOptions.Sugar;
            
            var enumValue = (CoffeeOptions)Enum.ToObject(typeof(CoffeeOptions), 12); // 4 + 8

            Assert.AreEqual(expected, enumValue);
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

        /*
         * enums cannot inherit from an enum or a class.
         * 
         * compile errors:
         *
         *   enum DerivedFromEnum : AddressType { }
         *   enum DerivedFromClass : String { }
         * 
         * enums are sealed:
         * 
         *   class DerivedClass : AddressType { }
        */
    }
}
