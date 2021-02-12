using System;

namespace Sharplus.Linq
{
    internal static class Error
    {
        public static Exception ArgumentNull(string argumentName)
        {
            return new ArgumentNullException($"Value cannot be null. (Parameter '{argumentName}')");
        }

        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        public static Exception NotEnoughElements(int min)
        {
            return new InvalidOperationException($"Sequence contains not enough elements. (Minimum required: {min})");
        }
    }
}
