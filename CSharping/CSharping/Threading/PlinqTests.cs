using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace CSharping.Threading
{
    [TestFixture]
    public class PlinqTests
    {
        [Test]
        public void AsParallel_Linq()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 100000);

            ParallelQuery<int> parallelQuery =
              from n in numbers.AsParallel()
              where n % 2 == 0
              select n;

            int[] evenNumbers = parallelQuery.ToArray();

            Assert.AreEqual(50000, evenNumbers.Length);
        }

        [Test]
        public void AsParallel_LinqMethodChain()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 100000);

            ParallelQuery<int> parallelQuery = numbers
                .AsParallel()
                .Where(n => n % 2 == 0)
                .Select(n => n);

            int[] evenNumbers = parallelQuery.ToArray();

            Assert.AreEqual(50000, evenNumbers.Length);
        }

        [Test]
        public void AsParallel_LinqMethodChain_AsSequential()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 100000);

            IEnumerable<int> sequentialEnumeration = numbers
                .AsParallel()
                .Where(n => n % 2 == 0)
                .AsSequential()
                .Where(n => n % 4 == 0)
                .Select(n => n);

            int[] evenNumbers = sequentialEnumeration.ToArray();

            Assert.AreEqual(25000, evenNumbers.Length);
        }

        [Test]
        public void AsOrdered()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 100000).Reverse();

            ParallelQuery<int> parallelQuery = numbers.AsParallel()
                .AsOrdered() // needed to preserve ordering in a parallel query. Incurs performance hit.
                .TakeWhile(n => n > 50000) // cannot be parallelized unless elements ordered
                .AsUnordered()
                .Where(n => n%2 == 0)
                .Select(n => n);

            int[] evenNumbersLargerThan50000 = parallelQuery.ToArray();

            Assert.AreEqual(25000, evenNumbersLargerThan50000.Length);
        }

        [Test]
        public void ThreadLocal_ForNonThreadSafeClasses()
        {
            // class Random is not thread safe; use a new instance on each thread.
            var localRandom = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
            int[] numbers = Enumerable.Range(1, 100000).ToArray();


            var randomNumbers = Enumerable.Range(0, 100)
                .AsParallel()
                .Select(n => numbers[localRandom.Value.Next(0, numbers.Length)])
                .ToArray();

            Assert.AreEqual(100, randomNumbers.Length);
        }

        [Test]
        public void WithDegreeOfParallelism()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 100000);

            var filteredNumbers = numbers
                .AsParallel()
                .WithDegreeOfParallelism(6) // runs 6 tasks simultaneously
                .Where(n => n < 10)
                .ToArray();
            
            Assert.AreEqual(9, filteredNumbers.Length);
        }

        [Test]
        [ExpectedException(typeof(OperationCanceledException))]
        public void WithCancellation_ThrowsWhenCanceled()
        {
            var cancellationSource = new CancellationTokenSource();
            IEnumerable<int> numbers = Enumerable.Range(1, 1000000);

            var queryToCancel = numbers
                .AsParallel()
                .WithCancellation(cancellationSource.Token)
                .Where(n => n*n < n + n)    // give the parallel tasks something to compute
                .Select(n => Math.Sqrt(n));

            new Thread(() =>
            {
                Thread.Sleep(1);
                cancellationSource.Cancel();
            }).Start();

            queryToCancel.ToArray();
        }

        [Test]
        public void WhenNeedToEnumerateParallelResult_OrderIsNotImportant_UseForAll()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 100000);

            numbers.AsParallel().Select(n => Math.Sinh(n)).ForAll(n => { /* do stuff */ ;});
        }
    }
}
