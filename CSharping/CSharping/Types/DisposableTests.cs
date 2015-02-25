using System;
using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/system.idisposable%28v=vs.110%29.aspx

    [TestFixture]
    public class DisposableTests
    {
        [Test]
        public void SimpleDisposable_Using_CallsDispose()
        {
            SimpleDisposable simpleDisposable;
            using (simpleDisposable = new SimpleDisposable())
            {
                // do stuff
            }

            Assert.IsTrue(simpleDisposable.WasDisposed);
        }

        class SimpleDisposable : IDisposable
        {
            public void Dispose()
            {
                // free unmanaged resources

                // free managed resources

                WasDisposed = true;
            }

            public bool WasDisposed { get; private set; }
        }
    }
}
