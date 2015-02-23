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
            var impl = new ClassWithExplicitImplementation();

            string result = ((ISayHi)impl).Greet();
            // compile error: method does not exist 
            // string result = impl.TGreet();

            Assert.AreEqual(result, "hi");
        }

        class ClassWithExplicitImplementation : ISayHi
        {
            // explicit implementation
            string ISayHi.Greet()
            {
                return "hi";
            }
        }

        [Test]
        public void TwoExplicitImplementations()
        {
            var impl = new ClassWithTwoExplicitImplementations();

            string hello = ((ISayHello)impl).Greet();
            string hi = ((ISayHi)impl).Greet();

            Assert.AreEqual("hello", hello);
            Assert.AreEqual("hi", hi);
        }

        class ClassWithTwoExplicitImplementations : ISayHi, ISayHello
        {
            string ISayHi.Greet()
            {
                return "hi";
            }

            string ISayHello.Greet()
            {
                return "hello";
            }
        }

        interface ISayHi
        {
            string Greet();
        }

        interface ISayHello
        {
            string Greet();
        }
    }
}
