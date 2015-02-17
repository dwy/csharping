using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class IndexerTests
    {
        [Test]
        public void Indexer_Get()
        {
            var list = new IndexedIntList(1, 2, 3);

            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [Test]
        public void Indexer_Set()
        {
            var list = new IndexedIntList(1, 2, 3);
            list[1] = 20;

            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(20, list[1]);
            Assert.AreEqual(3, list[2]); 
        }

        class IndexedIntList
        {
            private readonly List<int> _list = new List<int>();

            public IndexedIntList(params int[] values)
            {
                _list.AddRange(values);
            }

            public int this[int index]
            {
                get { return _list[index]; }
                set { _list[index] = value; }
            }
        }
    }
}
