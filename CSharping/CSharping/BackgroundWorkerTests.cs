using System.ComponentModel;
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
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;

            worker.RunWorkerAsync("work");
        }

        private void DoWork(object sender, DoWorkEventArgs args)
        {
            args.Result = args.Argument + " finished";
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            string result = (string) args.Result;
            Assert.AreEqual("work finished", result);
        }
    }
}
