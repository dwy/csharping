using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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

        [Test]
        public void DynamicContainer_DynamicallySetMembers()
        {
            dynamic container = new DynamicContainer();

            container.DynamicMember1 = 42;
            container.DynamicMember2 = "Bob";

            Assert.AreEqual(2, container.Count);
            Assert.AreEqual(42, container.DynamicMember1);
            Assert.AreEqual("Bob", container.DynamicMember2);
        }

        [Test]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void DynamicContainer_InexistentMember_Throws()
        {
            dynamic container = new DynamicContainer();

            Assert.AreEqual(42, container.InexistentMember);
        }

        // adapted from https://msdn.microsoft.com/en-us/library/system.dynamic.dynamicobject.aspx
        class DynamicContainer : DynamicObject, IEnumerable
        {
            private readonly Dictionary<string, object> _dynamicMembers = new Dictionary<string, object>();

            public int Count
            {
                get { return _dynamicMembers.Count; }
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                _dynamicMembers[binder.Name] = value;
                return true;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return _dynamicMembers.TryGetValue(binder.Name, out result);
            }

            public IEnumerator GetEnumerator()
            {
                return ((IEnumerable) _dynamicMembers).GetEnumerator();
            }
        }
    }
}
