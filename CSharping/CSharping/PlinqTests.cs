using System;
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

            ParallelQuery<int> parallelQuery =
                numbers.AsParallel().Where(n => n % 2 == 0)
                    .Select(n => n);

            int[] evenNumbers = parallelQuery.ToArray();

            Assert.AreEqual(50000, evenNumbers.Length);
        }
    }
}
