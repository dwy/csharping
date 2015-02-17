using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class MethodTests
    {
        [Test]
        public void Override_BaseMethodIsReplaced()
        {
            var child = new ChildOverride();

            string nameFromChild = child.GetName();
            string nameFromBase = ((Base)child).GetName();

            Assert.AreEqual("override", nameFromChild);
            Assert.AreEqual("override", nameFromBase);
        }

        [Test]
        public void NewKeyword_BaseMethodIsHidden()
        {
            var child = new ChildNew();

            string nameFromChild = child.GetName();
            string nameFromBase = ((Base)child).GetName();

            Assert.AreEqual("new", nameFromChild);
            Assert.AreEqual("base", nameFromBase);
        }

        class Base
        {
            public virtual string GetName() { return "base"; } 
        }

        class ChildOverride: Base
        {
            public override string GetName() { return "override"; }
        }

        class ChildNew : Base
        {
            public new string GetName() { return "new"; }
        }

        [Test]
        public void Overload_Unambiguous()
        {
            var over = new MethodOverloads();

            int value = over.Get(1);

            Assert.AreEqual(1, value);
        }

        [Test]
        public void Overload_IntInt()
        {
            var over = new MethodOverloads();

            string value = over.Get(1, 2);

            Assert.AreEqual("int int Get", value);
        }

        [Test]
        public void Overload_IntDouble()
        {
            var over = new MethodOverloads();

            string value = over.Get(1, 2.0);

            Assert.AreEqual("int double Get", value);
        }

        [Test]
        public void NamedParameters()
        {
            var over = new MethodOverloads();

            string value = over.Get(y: 2.0, x: 0);

            Assert.AreEqual("int double Get", value); 
        }

        class MethodOverloads
        {
            public int Get(int x)
            {
                return x;
            }

            public string Get(int x, int y)
            {
                return "int int Get";
            }

            public string Get(int x, double y)
            {
                return "int double Get";
            }
        }
    }
}
