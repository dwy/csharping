using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSharping.Types
{
    [TestFixture]
    public class ComparableTests
    {
        [Test]
        public void Comparable_SortList()
        {
            var values = new List<Comparable>
            {
                new Comparable("b"), new Comparable("A"), new Comparable("d"), new Comparable("c")
            };

            values.Sort();

            Assert.AreEqual("A", values[0].Value);
            Assert.AreEqual("b", values[1].Value);
            Assert.AreEqual("c", values[2].Value);
            Assert.AreEqual("d", values[3].Value);
        }

        class Comparable : IComparable
        {
            private readonly string _value;

            public Comparable(string value)
            {
                _value = value;
            }

            public string Value
            {
                get { return _value; }
            }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;

                Comparable c = obj as Comparable;
                if (c == null)
                {
                    throw new ArgumentException("not a Comparable");
                }
                return String.Compare(Value, c.Value, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Test]
        public void GenericComparable_SortList()
        {
            var values = new List<GenericComparable>
            {
                new GenericComparable(22), new GenericComparable(11), new GenericComparable(33), new GenericComparable(11)
            };

            values.Sort();

            Assert.AreEqual(11, values[0].Value);
            Assert.AreEqual(11, values[1].Value);
            Assert.AreEqual(22, values[2].Value);
            Assert.AreEqual(33, values[3].Value);
        }

        class GenericComparable : IComparable<GenericComparable>
        {
            private readonly int _value;

            public GenericComparable(int value)
            {
                _value = value;
            }

            public int Value
            {
                get { return _value; }
            }

            public int CompareTo(GenericComparable other)
            {
                if (other == null) return 1;
                if (_value > other._value) return 1;
                if (_value < other._value) return -1;
                return 0;
                // equivalent: return _value.CompareTo(other._value);
            }
        }

        [Test]
        public void GenericIComparer()
        {
            var values = new List<Data>
            {
                new Data(4), new Data(2), new Data(1), new Data(3),
            };

            var dataComparer = new GenericDataIComparer();
            values.Sort(dataComparer);

            Assert.AreEqual(1, values[0].Value);
            Assert.AreEqual(2, values[1].Value);
            Assert.AreEqual(3, values[2].Value);
            Assert.AreEqual(4, values[3].Value);
        }

        class Data
        {
            private readonly float _value;

            public Data(float value)
            {
                _value = value;
            }

            public float Value
            {
                get { return _value; }
            }
        }

        class GenericDataIComparer : IComparer<Data>
        {
            public int Compare(Data x, Data y)
            {
                if (x == null) return -1;
                if (y == null) return 1;
                return x.Value.CompareTo(y.Value);
            }
        }

        [Test]
        public void IComparer_SortArray()
        {
            var values = new []
            {
                new Data(4), new Data(2), new Data(1), new Data(3),
            };

            var dataComparer = new DataIComparer();
            Array.Sort(values, dataComparer);

            Assert.AreEqual(1, values[0].Value);
            Assert.AreEqual(2, values[1].Value);
            Assert.AreEqual(3, values[2].Value);
            Assert.AreEqual(4, values[3].Value);
        }

        class DataIComparer: IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null) return -1;
                if (y == null) return 1;

                var cx = x as Data;
                var cy = y as Data;

                if (cx == null || cy == null) throw new ArgumentException("should be an instance of Data");
                return cx.Value.CompareTo(cy.Value);
            }
        }

        [Test]
        public void Comparer_SortList()
        {
            var values = new List<Data>
            {
                new Data(4), new Data(2), new Data(1), new Data(3),
            };

            var dataComparer = new DataComparer();
            values.Sort(dataComparer);

            Assert.AreEqual(1, values[0].Value);
            Assert.AreEqual(2, values[1].Value);
            Assert.AreEqual(3, values[2].Value);
            Assert.AreEqual(4, values[3].Value); 
        }

        class DataComparer: Comparer<Data>
        {
            public override int Compare(Data x, Data y)
            {
                if (x == null) return -1;
                if (y == null) return 1;
                return x.Value.CompareTo(y.Value);
            }
        }
    }
}
