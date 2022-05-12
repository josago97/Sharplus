using System;
using System.Linq;

namespace System.Collections.Generic
{
    public class CircularArray<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _firstPosition;
        private int _nextFreePosition;
        private int _size;

        public int Length => _size;
        public int Capacity => _items.Length;

        public T this[int index]
        {
            get
            {
                if (index >= _size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _items[GetLocalIndex(index)];
            }

            set
            {
                if (index >= _size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _items[GetLocalIndex(index)] = value;
            }
        }

        public CircularArray(int capacity)
        {
            _items = new T[capacity];
            Clear();
        }

        public CircularArray(IEnumerable<T> collection)
        {
            Clear();
            _items = collection.ToArray();
            _size = _items.Length;
        }

        public void Add(T item)
        {
            _items[_nextFreePosition] = item;

            _nextFreePosition++;

            if (_nextFreePosition == _items.Length) _nextFreePosition = 0;

            if (_size < _items.Length) _size++;
            else
            {
                _firstPosition++;
                if (_firstPosition == _items.Length) _firstPosition = 0;
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                Add(item);
            }
        }

        public T GetValue(int index)
        {
            return _items[index];
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            _firstPosition = 0;
            _nextFreePosition = 0;
            _size = 0;
        }

        public T[] ToArray()
        {
            return _items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetLocalIndex(int index)
        {
            if (index >= Length) throw new IndexOutOfRangeException();

            int localIndex = _firstPosition + index;
            if (localIndex >= Length) localIndex -= Length;

            return localIndex;
        }

        private class Enumerator : IEnumerator<T>
        {
            private CircularArray<T> _pipe;
            private int _index;

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public Enumerator(CircularArray<T> pipe)
            {
                _pipe = pipe;
                Reset();
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < _pipe.Length)
                {
                    Current = _pipe._items[_pipe.GetLocalIndex(_index)];
                    _index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                _index = 0;
                Current = default;
            }
        }
    }
}
