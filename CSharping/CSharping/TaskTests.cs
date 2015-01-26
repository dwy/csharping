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
            taskA.Wait();
            queue.AddMessage(taskA.Result + " finished");

            // ordering of messages can vary
            var messages = queue.GetAll();
            CollectionAssert.Contains(messages, "independent work");
            CollectionAssert.Contains(messages, "Task A");
            CollectionAssert.Contains(messages, "Task B");
            CollectionAssert.Contains(messages, "Task B finished");
            CollectionAssert.Contains(messages, "Task A finished");
        }

        [Test]
        public void StartNew_StateObject()
        {
            var queue = new MessageQueue();
            Task<string> taskA = Task.Factory.StartNew<string>(DoWorkWithState, "Task A");

            // task.Result awaits the task
            DoWork(taskA.Result + " finished", queue);

            var messages = queue.GetAll();
            CollectionAssert.Contains(messages, "Task A finished");
            Assert.AreEqual("Task A", taskA.AsyncState);
        }

        private string DoWorkWithState(object asyncState)
        {
            return asyncState as string;
        }

        [Test]
        public void StartNew_LambdaAndStateObject()
        {
            var queue = new MessageQueue();
            var taskA = Task.Factory.StartNew(state => DoWork("Task A", queue), "asyncState for Task A");

            // task.Result awaits the task
            DoWork(taskA.Result + " finished", queue);

            var messages = queue.GetAll();
            CollectionAssert.Contains(messages, "Task A finished");
            Assert.AreEqual("asyncState for Task A", taskA.AsyncState);
        }

        [Test]
        public async void NewTask_Start()
        {
            var queue = new MessageQueue();
            var taskA = new Task<string>(() => DoWork("Task A", queue));
            var taskB = new Task<string>(() => DoWork("Task B", queue));

            taskA.Start();
            taskB.Start();

            // await the tasks
            await Task.WhenAll(taskA, taskB);
            // equivalent to: Task.WaitAll(taskA, taskB);

            // ordering of messages can vary
            var messages = queue.GetAll();
            CollectionAssert.Contains(messages, "Task A");
            CollectionAssert.Contains(messages, "Task B");
        }

        [Test]
        public void NewTask_TaskCreationOptions()
        {
            var queue = new MessageQueue();
            // for long, blocking Tasks
            var taskA = new Task<string>(() => DoWork("Task A", queue), TaskCreationOptions.LongRunning);
            // schedule Tasks in the order they were created, when possible
            var taskB = new Task<string>(() => DoWork("Task B", queue), TaskCreationOptions.PreferFairness); 

            taskA.Start();
            taskB.Start();

            Task.WaitAll(taskA, taskB);

            // ordering of messages can vary
            var messages = queue.GetAll();
            CollectionAssert.Contains(messages, "Task A");
            CollectionAssert.Contains(messages, "Task B");
        }

        [Test]
        public async void ChildTask_AwaitParent_ChildIsAwaited()
        {
            var queue = new MessageQueue();
            Task parent = Task.Factory.StartNew(() =>
            {
                DoWork("parent task", queue);

                Task.Factory.StartNew(() => DoWork("detached task", queue));

                Task.Factory.StartNew(() => DoWork("child task", queue), TaskCreationOptions.AttachedToParent);
            });

            await parent;

            var messages = queue.GetAll();
            CollectionAssert.Contains(messages, "parent task");
            CollectionAssert.Contains(messages, "child task");
            // detached Task is independent from 'parent'
        }


        private string DoWork(string name, MessageQueue queue)
        {
            queue.AddMessage(name);
            return name;
        }
    }
}
