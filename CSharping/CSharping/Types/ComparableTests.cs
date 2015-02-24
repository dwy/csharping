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
    }
}
