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

        [Test]
        public void StringIndexer_Get_IndexFound()
        {
            var stringIndexer = new StringIndexer(new Dictionary<string, int>
            {
                {"Alice", 22},
                {"Bob", 42},
            });

            Assert.AreEqual(22, stringIndexer["Alice"]);
            Assert.AreEqual(42, stringIndexer["Bob"]);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void StringIndexer_Get_IndexNotFound_Throws()
        {
            var stringIndexer = new StringIndexer(new Dictionary<string, int>
            {
                {"Alice", 22},
            });

            Assert.AreEqual(999, stringIndexer["Inexistent"]);
        }

        [Test]
        public void StringIndexer_Set()
        {
            var stringIndexer = new StringIndexer(new Dictionary<string, int>
            {
                {"Alice", 22},
                {"Bob", 42},
            });
            stringIndexer["Alice"] = 222;
            stringIndexer["Calvin"] = 333;

            Assert.AreEqual(222, stringIndexer["Alice"]);
            Assert.AreEqual(42, stringIndexer["Bob"]);
            Assert.AreEqual(333, stringIndexer["Calvin"]);
        }

        class StringIndexer
        {
            private readonly Dictionary<string, int> _dictionary;

            public StringIndexer(Dictionary<string, int> dictionary)
            {
                _dictionary = dictionary;
            }

            public int this[string name]
            {
                get { return _dictionary[name]; }
                set { _dictionary[name] = value; }
            }
        }
    }
}
