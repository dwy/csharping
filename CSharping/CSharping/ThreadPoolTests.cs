using System;
using System.Threading;
using NUnit.Framework;

namespace CSharping
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

        private string DoWork(string input)
        {
            return input + " finished";
        }

        private void DoWork(object state)
        {

        }
    }
}
