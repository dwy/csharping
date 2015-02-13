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

        enum Days
        {
            Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
        }
    }
}
