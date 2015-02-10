using System;
using System.ComponentModel;
using System.Threading;
using NUnit.Framework;

namespace CSharping
{
    [TestFixture]
    public class BackgroundWorkerTests
    {
        [Test]
        public void BackgroundWorker()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                args.Result = args.Argument + " finished";
            };
            
            worker.RunWorkerCompleted += (sender1, args1) => Assert.AreEqual("work finished", args1.Result);

            worker.RunWorkerAsync("work");
        }

        [Test]
        public void BackgroundWorker_ReportsProgress()
        {
            var worker = new BackgroundWorker { WorkerReportsProgress = true };
            worker.DoWork += DoWorkWithProgressReport;
            worker.ProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage + "% finished");
            worker.RunWorkerCompleted += (sender1, args1) => Assert.AreEqual("work in progress finished with 100%", args1.Result);

            worker.RunWorkerAsync("work in progress");
        }

        private void DoWorkWithProgressReport(object sender, DoWorkEventArgs args)
        {
            var worker = (BackgroundWorker) sender;
            for (int i = 0; i <= 10; i++)
            {
                worker.ReportProgress(i * 10);
            }
            args.Result = string.Format("{0} finished with 100%", args.Argument);
        }

        [Test]
        public void BackgroundWorker_SupportsCancellation()
        {
            var worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            worker.DoWork += DoWorkWithCancellation;
            worker.ProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage + "% finished");
            worker.RunWorkerCompleted += (sender, args) => Assert.IsTrue(args.Cancelled);

            worker.RunWorkerAsync("work to cancel");

            worker.CancelAsync();
        }

        private void DoWorkWithCancellation(object sender, DoWorkEventArgs args)
        {
            var worker = (BackgroundWorker) sender;
            for (int i = 0; i < 10; i++)
            {
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                Thread.Sleep(100);
            }
            args.Result = args.Argument + " finished";
        }
    }
}
