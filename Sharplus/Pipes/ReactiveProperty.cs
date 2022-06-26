using System;

namespace Sharplus.Pipes
{
    public class ReactiveProperty<T> : IEquatable<ReactiveProperty<T>>, IComparable<ReactiveProperty<T>>, IEquatable<T>, IComparable<T>
    {
        private T _value;

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
        }

        public ReactiveProperty() : this(default)
        {
        }

        public override string ToString() => _value.ToString();
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();

        public bool Equals(ReactiveProperty<T> other) => _value.Equals(other.Value);
        public int CompareTo(ReactiveProperty<T> other) => ((IComparable)_value).CompareTo(other.Value);

        public bool Equals(T other) => _value.Equals(other);

        public int CompareTo(T other) => ((IComparable)_value).CompareTo(other);

        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Value;

        private bool Equals(T value1, T value2)
        {
            bool res;

            if (value1 == null)
            {
                res = value2 == null;
            }
            else
            {
                res = value1.Equals(value2);
            }

            return res;
        }
    }
}
