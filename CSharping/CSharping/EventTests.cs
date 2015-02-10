﻿using System;
using NUnit.Framework;

namespace CSharping
{
    // https://msdn.microsoft.com/en-us/library/aa645739%28v=vs.71%29.aspx
    
    public delegate void RunningEventHandler(object sender, string message, EventArgs args);

    [TestFixture]
    public class EventTests
    {
        [Test]
        public void SimpleEvent_RegisterDelegate()
        {
            var classWithEvent = new ClassWithEvent();
            classWithEvent.WasRun += delegate(object sender, string message, EventArgs args)
            {
                Assert.AreEqual("run with message", message);
            };

            classWithEvent.Run("run with message");
        }

        [Test]
        public void SimpleEvent_RegisterLambda()
        {
            var classWithEvent = new ClassWithEvent();
            classWithEvent.WasRun += (sender, message, args) => Assert.AreEqual("run with message", message);

            classWithEvent.Run("run with message");
        }

        [Test]
        public void SimpleEvent_RegisterNamedMethod()
        {
            var classWithEvent = new ClassWithEvent();
            classWithEvent.WasRun += WasRunEventHandler;

            classWithEvent.Run("run with message");
        }

        [Test]
        public void SimpleEvent_RegisterNewEventHandler()
        {
            var classWithEvent = new ClassWithEvent();
            classWithEvent.WasRun += new RunningEventHandler(WasRunEventHandler);

            classWithEvent.Run("run with message");
        }

        private void WasRunEventHandler(object sender, string message, EventArgs args)
        {
            Assert.AreEqual("run with message", message);
        }

        sealed class ClassWithEvent
        {
            public event RunningEventHandler WasRun;

            public void Run(string message)
            {
                var handler = WasRun;
                if (handler != null) handler(this, message, EventArgs.Empty);
            }
        }

    }
}
