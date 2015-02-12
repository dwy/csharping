using System;
using NUnit.Framework;

namespace CSharping
{
    public delegate void MessageActionDelegate(string message);
    public delegate int MessageFunctionDelegate(string message);

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

        [Test]
        public void CallFunctionDelegate_PassLambda()
        {
            var functionDelegate = new MessageFunctionDelegate(m => m.Length);

            int length = functionDelegate("hi");

            Assert.AreEqual(2, length);
        }

        [Test]
        public void CallFunctionDelegate_PassFunction()
        {
            var functionDelegate = new MessageFunctionDelegate(GetMessageLength);

            int length = functionDelegate("hi");

            Assert.AreEqual(2, length);
        }

        [Test]
        public void PassFunctionDelegateAsParameter()
        {
            var functionDelegate = new MessageFunctionDelegate(GetMessageLength);

            int length = ExecuteMessageFunction(functionDelegate, "hi");

            Assert.AreEqual(2, length);
        }

        private int ExecuteMessageFunction(MessageFunctionDelegate functionDelegate, string message)
        {
            return functionDelegate(message);
        }

        [Test]
        public void PassBuiltinFunctionDelegateAsParameter()
        {
            int length = ExecuteFunction(GetMessageLength, "hi");

            Assert.AreEqual(2, length);
        }

        private int ExecuteFunction(Func<string, int> functionDelegate, string message)
        {
            return functionDelegate(message);
        }

        private static int GetMessageLength(string m)
        {
            return m.Length;
        }

        [Test]
        public void PassBuiltinFunctionDelegateAsParameter_SeveralParameters()
        {
            int length = ExecuteFunction(GetTotalLength, "hi", "there", "bob");

            Assert.AreEqual(10, length);
        }

        private int ExecuteFunction(Func<string, string, string, int> functionDelegate, string message1, string message2, string message3)
        {
            return functionDelegate(message1, message2, message3);
        }

        private int GetTotalLength(string arg1, string arg2, string arg3)
        {
            return (arg1 + arg2 + arg3).Length;
        }
    }
}
