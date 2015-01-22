using NUnit.Framework;
using System.Threading.Tasks;

namespace CSharping
{
    [TestFixture]
    public class ParallelTests
    {
        [Test]
        public void Parallel_Invoke()
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

        private void DoWork(string name, MessageQueue queue)
        {
            queue.AddMessage("work {0}", name);
        }
    }
}
