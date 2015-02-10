using System;
using NUnit.Framework;

namespace CSharping
{
    // https://msdn.microsoft.com/en-us/library/aa645739%28v=vs.71%29.aspx
    
    public delegate void RunningEventHandler(object sender, string message, EventArgs args);

    [TestFixture]
    public class EventTests
    {
        [Test]
        public void SimpleEvent()
        {
            var classWithEvent = new ClassWithEvent();
            classWithEvent.OnRunning += delegate(object sender, string message, EventArgs args)
            {
                Assert.AreEqual("run with message", message);
            };

            classWithEvent.Run("run with message");
        }

        sealed class ClassWithEvent
        {
            public event RunningEventHandler OnRunning;

            public void Run(string message)
            {
                var handler = OnRunning;
                if (handler != null) handler(this, message, EventArgs.Empty);
            }
        }

    }
}
