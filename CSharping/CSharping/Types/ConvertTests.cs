using System;
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

    }
}
