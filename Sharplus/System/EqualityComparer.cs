using System.Collections;

namespace System
{
    public interface IEqualityComparer<T, G> : IEqualityComparer
    {
        public bool Equals(T t, G g);
    }

    public class EqualityComparer<T, G> : IEqualityComparer<T, G>
    {
        private Func<T, G, bool> _equals;

        public static EqualityComparer<T, G> Default => new EqualityComparer<T, G>();

        public EqualityComparer(Func<T, G, bool> equals)
        {
            _equals = equals;
        }

        public EqualityComparer() : this((T t, G g) => t.Equals(g))
        {
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="t">The element of type <see cref="T"/> to compare.</param>
        /// <param name="g">The element of type <see cref="G"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(T t, G g)
        {
            return _equals(t, g);
        }

        public new bool Equals(object x, object y)
        {
            if (x is T t && y is G g) return _equals(t, g);
            else return false;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
