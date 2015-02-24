using System;
using System.Collections.Generic;
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
    }
}
