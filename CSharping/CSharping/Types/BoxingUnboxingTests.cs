using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/yz2be5wk.aspx

    [TestFixture]
    public class BoxingUnboxingTests
    {
        [Test]
        public void Boxing_ValidUnboxing()
        {
            const int value = 42;

            object boxed = value; // implicitly box integer in an object
            int unboxed = (int) boxed; // unbox explicitly

            Assert.AreEqual(42, boxed);
            Assert.AreEqual(42, unboxed);
            Assert.AreEqual(42, value);
        }
    }
}
