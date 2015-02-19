using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/dd264736.aspx

    [TestFixture]
    public class DynamicTests
    {
        [Test]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void InexistentMethod_CompilesButThrows()
        {
            dynamic dyn = new MyClass();

            dyn.InexistentMethod();
        }

        [Test]
        public void MethodExists_ResolvedAtRuntime()
        {
            dynamic dyn = new MyClass();

            dynamic result = dyn.Get(1);

            Assert.AreEqual(1, result);
            Assert.IsInstanceOf<int>(result);
        }

        class MyClass
        {
            public string Get(string s)
            {
                return s;
            }

            public int Get(int i)
            {
                return i;
            }
        }
    }
}
