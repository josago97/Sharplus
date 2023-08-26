using System;

namespace Sharplus.System
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns a subarray that contains all elements within a range.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="startIndex">The zero-based starting index of the range.</param>
        /// <param name="length">The number of elements to take.</param>
        /// <returns>
        /// An array with the elements that are present in the range.
        /// </returns>
        public static TSource[] SubArray<TSource>(this TSource[] source, int startIndex, int length)
        {
            int count = Math.Min(length, source.Length - startIndex);

            return new ArraySegment<TSource>(source, startIndex, count).ToArray();
        }

        /// <summary>
        /// Returns a subarray that contains all elements within a range.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="startIndex">The zero-based starting index of the range.</param>
        /// <returns>
        /// An array with the elements that are present in the range.
        /// </returns>
        public static TSource[] SubArray<TSource>(this TSource[] source, int startIndex)
        {
            return source.SubArray(startIndex, source.Length - startIndex);
        }


    }
}
