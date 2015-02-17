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
    }
}
