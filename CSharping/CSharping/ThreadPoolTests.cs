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

        private void DoWork(object state)
        {

        }
    }
}
