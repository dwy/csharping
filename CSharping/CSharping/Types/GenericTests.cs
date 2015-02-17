using System;
using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/sz6zd40f.aspx

    [TestFixture]
    public class GenericTests
    {
        [Test]
        public void Simple()
        {
            var g = new GenericClass<string>();

            Type type = g.GetParameterType();

            Assert.AreEqual(typeof (string), type);
        }

        class GenericClass<T>
        {
            public Type GetParameterType()
            {
                return typeof(T);
            }
        }
    }
}
