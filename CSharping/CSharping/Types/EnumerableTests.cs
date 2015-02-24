using System;
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
            Assert.AreEqual(3, result[3]);
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
                for (int i = 0; i <= _count; i++) // compiler generates enumerator class
                {
                    yield return i;
                }
            }
        }

        [Test]
        public void GenericIEnumerable_YieldReturn_CompilerGeneratesEnumerator()
        {
            var counter = new GenericCountTo(3);

            var result = counter.ToList();

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
            Assert.AreEqual(3, result[3]);
        }

        class GenericCountTo: IEnumerable<int>
        {
            private readonly int _count;

            public GenericCountTo(int count)
            {
                _count = count;
            }

            public IEnumerator<int> GetEnumerator()
            {
                for (int i = 0; i <= _count; i++) // compiler generates enumerator class
                {
                    yield return i;
                }
            }

            // need to implement the non-generic IEnumerable because IEnumerable<T> implements IEnumerable
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Test]
        public void CustomListIterator_ToList_EnumeratorIsUsed()
        {
            var customList = new CustomList<int> { 1, 2, 3 };

            // also using enumerator: ToArray(), ToDictionary(), ToLookup()
            List<int> newList = customList.ToList();

            Assert.IsTrue(customList.WasIterated);
            Assert.AreEqual(1, newList[0]);
            Assert.AreEqual(2, newList[1]);
            Assert.AreEqual(3, newList[2]);
        }

        [Test]
        public void CustomListIterator_Select_EnumeratorIsNotUsed()
        {
            var customList = new CustomList<int> { 1, 2, 3 };

            IEnumerable<int> incrementedElements = customList.Select(n => n + 1);

            Assert.IsFalse(customList.WasIterated);
        }

        class CustomList<T>: IEnumerable<T>
        {
            private bool _wasIterated = false;
            private readonly List<T> _list = new List<T>(); 

            public IEnumerator<T> GetEnumerator()
            {
                _wasIterated = true;
                return new ListIterator<T>(_list);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public bool WasIterated
            {
                get { return _wasIterated; }
            }

            // used by the collection initialiser
            public void Add(T element)
            {
                _list.Add(element);
            }
        }

        class ListIterator<T> : IEnumerator<T>
        {
            private readonly IList<T> _list;
            private int _currentPosition = -1;

            public ListIterator(IList<T> list)
            {
                _list = list;
            }

            public bool MoveNext()
            {
                _currentPosition++;
                return _currentPosition < _list.Count;
            }

            public void Reset()
            {
                _currentPosition = -1;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public T Current
            {
                get
                {
                    try
                    {
                        return _list[_currentPosition];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        throw new InvalidOperationException("Invalid cursor position", e);
                    }
                }
            }

            public void Dispose() { }
        }
    }
}
