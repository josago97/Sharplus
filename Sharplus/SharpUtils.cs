namespace Sharplus
{
    public static class SharpUtils
    {
        /// <summary>
        /// Swap references between two objects.
        /// </summary>
        /// <param name="object1">First object.</param>
        /// <param name="object2">Second object</param>
        public static void SwapReferences<T>(ref T object1, ref T object2)
        {
            (object1, object2) = (object2, object1);
        }
    }
}
