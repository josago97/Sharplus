using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /*public class AnonymousComparer
    {
        #region IComparer<T>
        
        public static IComparer<object> Create<T, G>(Func<T, G, int> compare)
        {
            if (compare == null) throw new ArgumentNullException(nameof(compare));

            return new Comparer<T, G>(compare);
        }

        private class Comparer<T, G> : IComparer<object>
        {
            private readonly Func<T, G, int> _compare;

            public Comparer(Func<T, G, int> compare)
            {
                _compare = compare;
            }

            public int Compare(object x, object y)
            {
                return _compare((T)x, (G)y);
            }
        }

        #endregion

        #region IEqualityComparer<T>

        public static IEqualityComparer<object> Create<T, G>(Func<T, G, bool> equals)
        {
            if (equals == null) throw new ArgumentNullException(nameof(equals));

            return new EqualityComparer<T, G>(
                equals,
                obj =>
                {
                    if (obj == null) return 0;
                    return obj.GetHashCode();
                });
        }

        private class EqualityComparer<T, G> : IEqualityComparer<object>
        {
            private readonly Func<T, G, bool> _equals;
            private readonly Func<T, int> _getHashCode;

            public EqualityComparer(Func<T, G, bool> equals, Func<T, int> getHashCode)
            {
                _equals = equals;
                _getHashCode = getHashCode;
            }

            public new bool Equals(object x, object y)
            {
                return _equals((T)x, (G)y);
            }

            public int GetHashCode(object obj)
            {
                return _getHashCode((T)obj);
            }
        }

        #endregion
    }*/
}
