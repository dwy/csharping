using System;
using System.Globalization;
using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class InterfaceTests
    {
        [Test]
        public void InterfaceImplementation()
        {
            var impl = new FormattableImplementation();

            string result = impl.ToString("hi", CultureInfo.InvariantCulture);

            Assert.AreEqual(result, "hi");
        }

        class FormattableImplementation : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
            {
                return format;
            }
        }

        [Test]
        public void ExplicitInterfaceImplementation_InstanceMustBeCast()
        {
            var impl = new FormattableExplicitImplementation();

            // compile error: method does not exist 
            // string result = impl.ToString("hi", CultureInfo.InvariantCulture);
            string result = ((IFormattable)impl).ToString("hi", CultureInfo.InvariantCulture);

            Assert.AreEqual(result, "hi");
        }

        class FormattableExplicitImplementation : IFormattable
        {
            // explicit IFormattabe implementation
            string IFormattable.ToString(string format, IFormatProvider formatProvider)
            {
                return format;
            }
        }
    }
}
