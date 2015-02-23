using System;
using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class ConvertTests
    {
        [Test]
        public void UNIT_SCENARIO_RESULT()
        {
            const string number = "1";

            int integer = (int) Convert.ChangeType(number, typeof (int));

            Assert.AreEqual(1, integer);
        }
    }
}
