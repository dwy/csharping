﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSharping
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

    }
}
