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
            var model = new AsyncModel();

            model.AddMessage("starting main");
            await SimulateWorkAsync("A", 10, model);
            model.AddMessage("after awaited task");

            var messages = model.GetMessages();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task A", messages[1]);
            Assert.AreEqual("task A ended", messages[2]);
            Assert.AreEqual("after awaited task", messages[3]);
        }

        [Test]
        public async void OneTask_WithIndependentWork()
        {
            var model = new AsyncModel();

            model.AddMessage("starting main");
            Task task = SimulateWorkAsync("A", 10, model);
            model.AddMessage("doing independent work");
            
            await task;

            var messages = model.GetMessages();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task A", messages[1]);
            Assert.AreEqual("doing independent work", messages[2]);
            Assert.AreEqual("task A ended", messages[3]);
        }

        [Test]
        public async void TwoTasks_Awaited()
        {
            var model = new AsyncModel();

            model.AddMessage("starting main");
            await SimulateWorkAsync("A", 10, model);
            await SimulateWorkAsync("B", 10, model);

            model.AddMessage("after awaited tasks");

            var messages = model.GetMessages();
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
            var model = new AsyncModel();

            model.AddMessage("starting main");
            Task shortTask = SimulateWorkAsync("short", 10, model);
            Task longTask = SimulateWorkAsync("long", 20, model);

            model.AddMessage("doing independent work");

            await Task.WhenAll(shortTask, longTask);

            var messages = model.GetMessages();
            Assert.AreEqual("starting main", messages[0]);
            Assert.AreEqual("starting task short", messages[1]);
            Assert.AreEqual("starting task long", messages[2]);
            Assert.AreEqual("doing independent work", messages[3]);
            Assert.AreEqual("task short ended", messages[4]);
            Assert.AreEqual("task long ended", messages[5]);
        }

        private async Task SimulateWorkAsync(string name, int ms, AsyncModel model)
        {
            model.AddMessage("starting task {0}", name);
            await Task.Delay(ms);
            model.AddMessage("task {0} ended", name);
        }
    }
}
