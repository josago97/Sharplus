using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sharplus.Collections
{
    public class CircularArray<T> : IList<T>, IReadOnlyList<T>
    {
        private T[] _items;
        private int _firstPosition;
        private int _nextFreePosition;
        private int _size;
        private IEqualityComparer<T> _comparer;

        public int Length => _size;
        public int Capacity => _items.Length;
        public int Count => Length;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                if (index >= _size)
                    throw new ArgumentOutOfRangeException();

                return _items[ToLocalIndex(index)];
            }

            set
            {
                if (index >= _size)
                    throw new ArgumentOutOfRangeException();

                _items[ToLocalIndex(index)] = value;
            }
        }

        private CircularArray()
        {
            Clear();
            _comparer = EqualityComparer<T>.Default;
        }

        public CircularArray(int capacity) : this()
        {
            _items = new T[capacity];
        }

        public CircularArray(IEnumerable<T> collection) : this()
        {
            _items = collection.ToArray();
            _size = _items.Length;
        }

        public void Add(T item)
        {
            _items[_nextFreePosition] = item;

            _nextFreePosition++;

            if (_nextFreePosition == _items.Length)
                _nextFreePosition = 0;

            if (_size < _items.Length)
                _size++;
            else
            {
                _firstPosition++;

                if (_firstPosition == _items.Length)
                    _firstPosition = 0;
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
                Add(item);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public int IndexOf(T item)
        {
            int result = -1;

            for (int i = 0; i < _size; i++)
            {
                if (_comparer.Equals(item, this[i]))
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();

            if (_size < _items.Length)
                _size++;

            // Make space
            for (int i = _size - 1; i > index; i--)
                this[i] = this[i - 1];

            // Store item
            this[index] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();

            for (int i = index; i < _size - 1; i++)
                this[i] = this[i + 1];

            _nextFreePosition--;
            if (_nextFreePosition < 0)
                _nextFreePosition = _items.Length - 1;

            _size--;
        }

        public bool Remove(T item)
        {
            bool result = false;
            int index = IndexOf(item);

            if (index != -1)
            {
                RemoveAt(index);
                result = true;
            }

            return result;
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            for (int i = 0; i < _size; i++)
                array.SetValue(this[i], arrayIndex + i);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < _size; i++)
                array[arrayIndex + i] = this[i];
        }

        public void Clear()
        {
            _firstPosition = 0;
            _nextFreePosition = 0;
            _size = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int ToLocalIndex(int index)
        {
            if (index >= _size)
                throw new IndexOutOfRangeException();

            int localIndex = _firstPosition + index;

            if (localIndex >= _items.Length)
                localIndex -= _items.Length;

            return localIndex;
        }

        private class Enumerator : IEnumerator<T>
        {
            private CircularArray<T> _circularArray;
            private int _index;

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public Enumerator(CircularArray<T> circularArray)
            {
                _circularArray = circularArray;
                Reset();
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                bool hasNext = false;

                if (_index < _circularArray.Length)
                {
                    Current = _circularArray[_index];
                    _index++;
                    hasNext = true;
                }

                return hasNext;
            }

            public void Reset()
            {
                _index = 0;
                Current = default;
            }
        }
    }
}
