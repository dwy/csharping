using NUnit.Framework;

namespace CSharping
{
    public delegate void MessageActionDelegate(string message);
    public delegate string MessageFunctionDelegate(string message);

    [TestFixture]
    public class DelegateTests
    {
        [Test]
        public void CallActionDelegate_PassLambda()
        {
            var queue = new MessageQueue();
            var actionDelegate = new MessageActionDelegate(m => { queue.AddMessage(m); });

            actionDelegate("hi");

            Assert.AreEqual("hi", queue.GetAll()[0]);
        }
    }
}
