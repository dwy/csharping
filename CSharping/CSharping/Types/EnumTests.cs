using System;
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
        public void DefaultUnderlyingTypeIsInt_StartsAtZero()
        {
            const int sundayNumber = (int) Days.Sunday;
            const int mondayNumber = (int) Days.Monday;

            Assert.AreEqual(0, sundayNumber);
            Assert.AreEqual(1, mondayNumber);
        }

        enum Days
        {
            Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
        }
    }
}
