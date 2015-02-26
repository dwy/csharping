using System;
using System.Collections.Generic;
using System.Globalization;
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

        [Test]
        public void DisposableExample_Using_CallsDispose()
        {
            DisposableResource disposable;
            using (disposable = new DisposableResource())
            {
                // do stuff
            }

            Assert.IsTrue(disposable.WasDisposed);
        }

        class DisposableResource : IDisposable
        {
            public bool WasDisposed { get; private set; }

            private IntPtr _unmanagedHandle ;
            private List<object> _managedList;

            public DisposableResource()
            {
                _unmanagedHandle = new IntPtr(42);
                _managedList = new List<object>
                {
                    new { name = "stuff", value = 123 },
                    1234,
                    CultureInfo.InvariantCulture
                };
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~DisposableResource()
            {
                Dispose(false);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (WasDisposed) return;

                if (disposing)
                {
                    if (_managedList != null)
                    {
                        _managedList.Clear();
                        _managedList = null;
                        // _otherManagedResource.Dispose();
                    }
                }

                CloseHandle(_unmanagedHandle);
                _unmanagedHandle = IntPtr.Zero;

                WasDisposed = true;
            }

            // Use interop to call the method necessary to clean up the unmanaged resource.
            [System.Runtime.InteropServices.DllImport("Kernel32")]
            private extern static Boolean CloseHandle(IntPtr handle);
        }

        [Test]
        public void DerivedDisposable_ImplementVirtualDispose_CallBaseMethod()
        {
            DerivedDisposableResource derivedDisposable;
            using (derivedDisposable = new DerivedDisposableResource())
            {
                // do stuff
            }

            Assert.IsTrue(derivedDisposable.WasDisposed);
        }

        class DerivedDisposableResource : DisposableResource
        {
            public new bool WasDisposed { get; private set; }

            protected override void Dispose(bool disposing)
            {
                if (WasDisposed) return;

                if (disposing)
                {
                    // free managed resources
                }

                // free unmanaged resources

                WasDisposed = true;

                // base classes resources must be freed too
                base.Dispose(disposing);
            }

            // do NOT override finaliser since base class already has one
            // ~DerivedDisposableResource() {}
        }
    }
}
