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
        public void WaitOnACanceledTask_ThrowsAggregateException()
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

        [Test]
        public void Continuation_IsExecutedAfterAntecedent()
        {
            var queue = new MessageQueue();
            Task<string> first = Task.Factory.StartNew(() => DoWork("first", queue));
            Task<string> continuation = first.ContinueWith(antecedentTask => DoWork("continuation after " + antecedentTask.Result, queue));

            Task.WaitAll(first, continuation);

            var messages = queue.GetAll();
            Assert.AreEqual("first", messages[0]);
            Assert.AreEqual("continuation after first", messages[1]);
        }

        [Test]
        public void Continuation_ExecuteOnTheSameThread()
        {
            var queue = new MessageQueue();
            Task<string> first = Task.Factory.StartNew(() => DoWork("first", queue));
            Task<string> continuation = first.ContinueWith(
                antecedentTask => DoWork("continuation after " + antecedentTask.Result, queue),
                TaskContinuationOptions.ExecuteSynchronously);

            Task.WaitAll(first, continuation);

            var messages = queue.GetAll();
            Assert.AreEqual("first", messages[0]);
            Assert.AreEqual("continuation after first", messages[1]);
        }

        [Test]
        public void Continuation_AntecedentThrows_ContinuationHasAccessToException()
        {
            Task first = Task.Factory.StartNew(() => { ThrowException("antecedent failed"); });
            Task<AggregateException> continuation = first.ContinueWith(antecedentTask =>  antecedentTask.Exception);

            AggregateException ex = continuation.Result;
            Assert.AreEqual("antecedent failed", ex.InnerException.Message);
        }

        [Test]
        public async void Continuation_AntecedentThrows_ErrorContinuationIsExecuted()
        {
            var queue = new MessageQueue();
            Task first = Task.Factory.StartNew(() => { ThrowException("antecedent failed"); });

            Task error = first.ContinueWith(antecedentTask =>
            {
                var ex = antecedentTask.Exception;
                if (ex != null)
                {
                    DoWork(ex.InnerException.Message, queue);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task success = first.ContinueWith(ant => DoWork("success", queue),
                                          TaskContinuationOptions.NotOnFaulted);

            await error;

            var messages = queue.GetAll();
            Assert.AreNotEqual(TaskStatus.RanToCompletion, success.Status);
            Assert.AreEqual("antecedent failed", messages[0]);
        }

        [Test]
        [ExpectedException(typeof(TaskCanceledException))]
        public async void Continuation_AntecedentThrows_SuccessContinuationIsCanceled()
        {
            var queue = new MessageQueue();
            Task first = Task.Factory.StartNew(() => { ThrowException("antecedent failed"); });

            Task error = first.ContinueWith(antecedentTask =>
            {
                var ex = antecedentTask.Exception;
                if (ex != null)
                {
                    DoWork(ex.InnerException.Message, queue);  
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task success = first.ContinueWith(antecedentTask => DoWork("success", queue),
                                          TaskContinuationOptions.NotOnFaulted);

            await success;
            await error;
        }

        [Test]
        public async void Continuation_AntecedentSucceeds_SuccessContinuationIsExecuted()
        {
            var queue = new MessageQueue();
            Task first = Task.Factory.StartNew(() => { DoWork("antecedent succeeded", queue); });

            Task error = first.ContinueWith(antecedentTask =>
            {
                var ex = antecedentTask.Exception;
                if (ex != null)
                {
                    DoWork(ex.InnerException.Message, queue);   
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task success = first.ContinueWith(ant => DoWork("success", queue),
                                          TaskContinuationOptions.NotOnFaulted);

            await success;

            var messages = queue.GetAll();
            Assert.AreNotEqual(TaskStatus.RanToCompletion, error.Status);
            Assert.AreEqual("antecedent succeeded", messages[0]);
        }

        [Test]
        [ExpectedException(typeof(TaskCanceledException))]
        public async void Continuation_AntecedentSucceeds_ErrorContinuationIsCanceled()
        {
            var queue = new MessageQueue();
            Task first = Task.Factory.StartNew(() => { DoWork("antecedent succeeded", queue); });

            Task error = first.ContinueWith(antecedentTask =>
            {
                var ex = antecedentTask.Exception;
                if (ex != null)
                {
                    DoWork(ex.InnerException.Message, queue); 
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task success = first.ContinueWith(antecedentTask => DoWork("success", queue),
                                          TaskContinuationOptions.NotOnFaulted);

            await error;
            await success;
        }

        // Conditional continuations

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
