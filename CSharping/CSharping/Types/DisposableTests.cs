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

        [Test]
        public void DisposablePattern_Using_CallsDispose()
        {
            DisposablePattern disposable;
            using (disposable = new DisposablePattern())
            {
                // do stuff
            }

            Assert.IsTrue(disposable.WasDisposed);
        }

        class DisposablePattern : IDisposable
        {
            public bool WasDisposed { get; private set; }

            public void Dispose()
            {
                Dispose(true);
                // do not call finalizer on this object, resources already cleaned up by Dispose(true).
                GC.SuppressFinalize(this);
            }

            // parameter 'disposing' true means that it is safe to clean up the managed resources too.
            protected virtual void Dispose(bool disposing)
            {
                if (WasDisposed) return;

                if (disposing)
                {
                    // free managed resources
                }

                // free unmanaged resources

                WasDisposed = true;
            }

            // finalizer
            ~DisposablePattern()
            {
                // when the finalizer is called, the managed resources might already have been cleaned up
                // by the garbage collector, hence calling Dispose with false.
                Dispose(false);
            }
        }
    }
}
