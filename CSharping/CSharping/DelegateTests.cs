using System;
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
            var actionDelegate = new MessageActionDelegate(m => queue.AddMessage(m));

            actionDelegate("hi");

            Assert.AreEqual("hi", queue.GetAll()[0]);
        }

        [Test]
        public void CallActionDelegate_PassMethod()
        {
            var queue = new MessageQueue();
            var actionDelegate = new MessageActionDelegate(queue.AddMessage);

            actionDelegate("hi");

            Assert.AreEqual("hi", queue.GetAll()[0]);
        }

        [Test]
        public void PassActionDelegateAsParameter()
        {
            var queue = new MessageQueue();
            var actionDelegate = new MessageActionDelegate(queue.AddMessage);

            ExecuteMessageAction(actionDelegate, "hi");

            Assert.AreEqual("hi", queue.GetAll()[0]);
        }

        private static void ExecuteMessageAction(MessageActionDelegate actionDelegate, string message)
        {
            actionDelegate(message);
        }

        [Test]
        public void PassBuiltinActionDelegateAsParameter()
        {
            var queue = new MessageQueue();

            ExecuteAction(queue.AddMessage, "hi");

            Assert.AreEqual("hi", queue.GetAll()[0]);
        }

        private static void ExecuteAction(Action<string> actionDelegate, string message)
        {
            actionDelegate(message);
        }
    }
}
