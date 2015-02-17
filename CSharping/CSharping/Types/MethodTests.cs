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

            string nameFromOverride = child.GetName();
            string nameFromBase = ((Base)child).GetName();

            Assert.AreEqual("override", nameFromOverride);
            Assert.AreEqual("override", nameFromBase);
        }

        class Base
        {
            public virtual string GetName() { return "base"; } 
        }

        class ChildOverride: Base
        {
            public override string GetName() { return "override"; }
        }

    }
}
