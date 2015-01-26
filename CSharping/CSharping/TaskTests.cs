using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharping
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void StartNew()
        {
            var queue = new MessageQueue();
            Task<string> taskA = Task.Factory.StartNew(() => DoWork("Task A", queue));
            Task<string> taskB = Task.Factory.StartNew(() => DoWork("Task B", queue));

            DoWork("independent work", queue);
            // await the tasks
            DoWork(taskB.Result + " finished", queue);
            DoWork(taskA.Result + " finished", queue);

            var messages = queue.GetAll();

            CollectionAssert.Contains(messages, "independent work");
            CollectionAssert.Contains(messages, "Task A");
            CollectionAssert.Contains(messages, "Task B");
            CollectionAssert.Contains(messages, "Task B finished");
            CollectionAssert.Contains(messages, "Task A finished");
        }



        private string DoWork(string name, MessageQueue queue)
        {
            queue.AddMessage(name);
            return name;
        }
    }
}
