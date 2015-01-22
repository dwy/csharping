using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharping
{
    [TestFixture]
    public class AsyncTests
    {
        [Test]
        public async void OneTask_Awaited()
        {
            var queue = new MessageQueue();

            queue.AddMessage("starting main");
            await SimulateWorkAsync("A", 10, queue);
            queue.AddMessage("after awaited task");

            var messages = queue.GetAll();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task A", messages[1]);
            Assert.AreEqual("task A ended", messages[2]);
            Assert.AreEqual("after awaited task", messages[3]);
        }

        [Test]
        public async void OneTask_WithIndependentWork()
        {
            var queue = new MessageQueue();

            queue.AddMessage("starting main");
            Task task = SimulateWorkAsync("A", 10, queue);
            queue.AddMessage("doing independent work");
            
            await task;

            var messages = queue.GetAll();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task A", messages[1]);
            Assert.AreEqual("doing independent work", messages[2]);
            Assert.AreEqual("task A ended", messages[3]);
        }

        [Test]
        public async void TwoTasks_Awaited()
        {
            var queue = new MessageQueue();

            queue.AddMessage("starting main");
            await SimulateWorkAsync("A", 10, queue);
            await SimulateWorkAsync("B", 10, queue);

            queue.AddMessage("after awaited tasks");

            var messages = queue.GetAll();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task A", messages[1]);
            Assert.AreEqual("task A ended", messages[2]);
            Assert.AreEqual("starting task B", messages[3]);
            Assert.AreEqual("task B ended", messages[4]);
            Assert.AreEqual("after awaited tasks", messages[5]);
        }

        [Test]
        public async void TwoTasks_WithIndependentWork()
        {
            var queue = new MessageQueue();

            queue.AddMessage("starting main");
            Task shortTask = SimulateWorkAsync("short", 10, queue);
            Task longTask = SimulateWorkAsync("long", 20, queue);

            queue.AddMessage("doing independent work");

            await Task.WhenAll(shortTask, longTask);

            var messages = queue.GetAll();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task short", messages[1]);
            Assert.AreEqual("starting task long", messages[2]);
            Assert.AreEqual("doing independent work", messages[3]);
            Assert.AreEqual("task short ended", messages[4]);
            Assert.AreEqual("task long ended", messages[5]);
        }

        [Test]
        public void OneTask_WithResult_ResultAwaitsTheTask()
        {
            var queue = new MessageQueue();

            queue.AddMessage("starting main");
            Task<string> task = SimulateWorkWithResultAsync("A", 10, queue);
            queue.AddMessage("doing independent work");
            queue.AddMessage("task.Result awaits the task. Result={0}", task.Result);

            var messages = queue.GetAll();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task A", messages[1]);
            Assert.AreEqual("doing independent work", messages[2]);
            Assert.AreEqual("task A ended", messages[3]);
            Assert.AreEqual("task.Result awaits the task. Result=A", messages[4]);
        }

        private async Task SimulateWorkAsync(string name, int ms, MessageQueue queue)
        {
            queue.AddMessage("starting task {0}", name);
            await Task.Delay(ms);
            queue.AddMessage("task {0} ended", name);
        }

        private async Task<string> SimulateWorkWithResultAsync(string name, int ms, MessageQueue queue)
        {
            await SimulateWorkAsync(name, ms, queue);
            return name;
        }
    }
}
