using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class EnumerableTests
    {
        [Test]
        public void IEnumerable_YieldReturn_CompilerGeneratesEnumerator()
        {
            var counter = new CountTo(3);

            var result = counter.Cast<int>().ToList();

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
        }

        class CountTo : IEnumerable
        {
            private readonly int _count;

            public CountTo(int count)
            {
                _count = count;
            }

            public IEnumerator GetEnumerator()
            {
                for (int i = 0; i <= _count; i++) // compiler generates enumerator
                {
                    yield return i;
                }
            }
        }
    }
}
