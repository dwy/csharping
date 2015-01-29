using System;
using System.Linq;
using System.Threading;
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

        [Test]
        public void AggregateException()
        {
            var queue = new MessageQueue();
            Task<string> taskA = Task.Factory.StartNew(() => ThrowException("Task A failed"));
            Task<string> taskB = Task.Factory.StartNew(() => DoWork("Task B", queue));
            Task<string> taskC = Task.Factory.StartNew(() => ThrowException("Task C failed"));

            try
            {
                Task.WaitAll(taskA, taskB, taskC);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(2, ex.InnerExceptions.Count);
                var exceptionMessages = ex.InnerExceptions.Select(e => e.Message).ToList();
                CollectionAssert.Contains(exceptionMessages, "Task A failed");
                CollectionAssert.Contains(exceptionMessages, "Task C failed");
            }

            CollectionAssert.Contains(queue.GetAll(), "Task B");
        }

        [Test]
        [ExpectedException(typeof(TaskCanceledException))]
        public async void AwaitACanceledTask_ThrowsTaskCanceledException()
        {
            var queue = new MessageQueue();
            var cancelSource = new CancellationTokenSource();

            Task<string> task = Task.Factory.StartNew(() => DoWork("work", queue), cancelSource.Token);

            cancelSource.Cancel();

            await task;
        }

        [Test]
        public async void TaskWaitOnACanceledTask_ThrowsAggregateException()
        {
            var cancelSource = new CancellationTokenSource();
            var token = cancelSource.Token;

            var taskToCancel = Task.Delay(1000, token);
            cancelSource.Cancel();

            try
            {
                taskToCancel.Wait(); // same as: Task.WaitAll(task);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(1, ex.InnerExceptions.Count);
                Assert.IsInstanceOf(typeof(TaskCanceledException), ex.InnerException);
            }
        }

        private string DoWork(string name, MessageQueue queue)
        {
            queue.AddMessage(name);
            return name;
        }

        private string ThrowException(string message)
        {
            throw new Exception(message);
        }
    }
}
