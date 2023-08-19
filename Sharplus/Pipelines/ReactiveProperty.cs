using System;
using System.Collections.Generic;

namespace Sharplus.Pipelines
{
    public class ReactiveProperty<T> : IEquatable<ReactiveProperty<T>>, IComparable<ReactiveProperty<T>>, IEquatable<T>, IComparable<T>
    {
        private T _value;
        private IComparer<T> _comparer;

        public event Action<T> ValueChanged;
        public event Action ValueChangedCallback;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    LastValue = _value;
                    _value = value;
                    ValueChanged?.Invoke(_value);
                    ValueChangedCallback?.Invoke();
                }
            }
        }

        public T LastValue { get; private set; }

        public ReactiveProperty(T value)
        {
            Value = value;
            _comparer = Comparer<T>.Default;
        }

        public ReactiveProperty() : this(default)
        {
        }

        public override string ToString() => _value?.ToString();
        public override int GetHashCode() => _value != null ? _value.GetHashCode() : 0;
        public override bool Equals(object obj)
        {
            bool result = false;

            if (ReferenceEquals(_value, obj))
                result = true;
            else if (_value != null)
                result = _value.Equals(obj);

            return result;
        }

        public bool Equals(T other) => Equals((object)other);
        public int CompareTo(T other) => _comparer.Compare(_value, other);
        public bool Equals(ReactiveProperty<T> other) => Equals(other.Value);
        public int CompareTo(ReactiveProperty<T> other) => CompareTo(other.Value);

        public static implicit operator T(ReactiveProperty<T> property) => property.Value;

        public static bool operator ==(ReactiveProperty<T> property, T value) => property.Equals(value);
        public static bool operator !=(ReactiveProperty<T> property, T value) => !(property == value);
        public static bool operator ==(T value, ReactiveProperty<T> property) => property == value;
        public static bool operator !=(T value, ReactiveProperty<T> property) => property != value;
        public static bool operator !=(ReactiveProperty<T> property1, ReactiveProperty<T> property2) => property1 != property2.Value;
        public static bool operator ==(ReactiveProperty<T> property1, ReactiveProperty<T> property2) => property1 == property2.Value;
    }
}
