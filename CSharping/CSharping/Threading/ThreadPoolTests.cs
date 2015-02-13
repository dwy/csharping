using System;
using System.Threading;
using NUnit.Framework;

namespace CSharping.Threading
{
    [TestFixture]
    public class ThreadPoolTests
    {
        [Test]
        public void QueueUserWorkItem()
        {
            ThreadPool.QueueUserWorkItem(state => Console.WriteLine("stuff:" + state));
            ThreadPool.QueueUserWorkItem(DoWork, 1234);
            ThreadPool.QueueUserWorkItem(DoWork, "stuff");
        }

        [Test]
        public void AsynchronousDelegate()
        {
            Func<string, string> func = DoWork;
            IAsyncResult asyncResult = func.BeginInvoke("func", null, null);

            string result = func.EndInvoke(asyncResult);

            Assert.AreEqual("func finished", result);
        }

        [Test]
        public void AsynchronousDelegate_WithCallBack()
        {
            Func<string, string> func = DoWork;
            var queue = new MessageQueue();
            IAsyncResult asyncResult = func.BeginInvoke("func", DoneCallBack, queue);

            string result = func.EndInvoke(asyncResult);

            Assert.AreEqual("func finished", result);
            // callback may still be running at this point
        }

        private string DoWork(string input)
        {
            return input + " finished";
        }

        private void DoneCallBack(IAsyncResult result)
        {
            var queue = (MessageQueue) result.AsyncState;
            queue.AddMessage("done callback called");
        }

        private void DoWork(object state)
        {

        }
    }
}
