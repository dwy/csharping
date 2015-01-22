using System;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CSharping
{
    [TestFixture]
    public class ParallelTests
    {
        [Test]
        public void Invoke()
        {
            var queue = new MessageQueue();
            
            Parallel.Invoke(
                () => DoWork("A", queue),
                () => DoWork("B", queue),
                () => DoWork("C", queue));

            var messages = queue.GetAll();
            // ordering of messages can vary
            CollectionAssert.Contains(messages, "work A");
            CollectionAssert.Contains(messages, "work B");
            CollectionAssert.Contains(messages, "work C");
        }

        [Test]
        [ExpectedException(typeof(OperationCanceledException))]
        public void Invoke_Canceled_Throws()
        {
            var queue = new MessageQueue();
            var cancelSource = new CancellationTokenSource();
            var options = new ParallelOptions { CancellationToken = cancelSource.Token };
            cancelSource.Cancel();

            Parallel.Invoke(options,
                () => DoWork("A", queue));
        }

        private void DoWork(string name, MessageQueue queue)
        {
            queue.AddMessage("work {0}", name);
        }
    }
}
