﻿using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharping.Threading
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

        [Test]
        public void For()
        {
            var queue = new MessageQueue();

            Parallel.For(0, 4, i => DoWork("" + i, queue));

            var messages = queue.GetAll();
            // ordering of messages can vary
            CollectionAssert.Contains(messages, "work 1");
            CollectionAssert.Contains(messages, "work 2");
            CollectionAssert.Contains(messages, "work 3");
            CollectionAssert.DoesNotContain(messages, "work 4");
        }

        [Test]
        public void ForEach()
        {
            var queue = new MessageQueue();
            var names = new[] {"A", "B", "C", "D"};

            Parallel.ForEach(names, name => DoWork(name, queue));

            var messages = queue.GetAll();
            // ordering of messages can vary
            CollectionAssert.Contains(messages, "work A");
            CollectionAssert.Contains(messages, "work B");
            CollectionAssert.Contains(messages, "work C");
            CollectionAssert.Contains(messages, "work D");
        }

        [Test]
        public void ForEach_Indexed()
        {
            var queue = new MessageQueue();
            var names = new[] { "A", "B", "C", "D" };

            Parallel.ForEach(names, 
                (name, state, i) => DoWork(name + i, queue));

            var messages = queue.GetAll();
            // ordering of messages can vary
            CollectionAssert.Contains(messages, "work A0");
            CollectionAssert.Contains(messages, "work B1");
            CollectionAssert.Contains(messages, "work C2");
            CollectionAssert.Contains(messages, "work D3");
        }

        [Test]
        public void ForEach_BreakOnNameC()
        {
            var queue = new MessageQueue();
            var names = new[] { "A", "B", "C", "D" };

            Parallel.ForEach(names,
                (name, state) => DoWorkBreakOnNameC(name, queue, state));

            var messages = queue.GetAll();
            // ordering of messages can vary
            CollectionAssert.Contains(messages, "work A");
            CollectionAssert.Contains(messages, "work B");
            CollectionAssert.DoesNotContain(messages, "work C");
            // task D will get finished if it was already started as the break was made
        }

        private void DoWorkBreakOnNameC(string name, MessageQueue queue, ParallelLoopState state)
        {
            if (string.Equals(name, "C"))
            {
                state.Break();
            }
            else
            {
                DoWork(name, queue);
            }
        }

        private void DoWork(string name, MessageQueue queue)
        {
            queue.AddMessage("work {0}", name);
        }
    }
}
