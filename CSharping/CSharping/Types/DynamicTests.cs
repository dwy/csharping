using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/dd264736.aspx
    // https://msdn.microsoft.com/en-us/library/dd264741.aspx

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
            dynamic result2 = dyn.Get("value");

            Assert.AreEqual(1, result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual("value", result2);
            Assert.IsInstanceOf<string>(result2);
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
