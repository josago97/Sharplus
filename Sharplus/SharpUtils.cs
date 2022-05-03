namespace Sharplus
{
    public static class SharpUtils
    {
        public static void SwapReferences<T>(ref T object1, ref T object2)
        {
            T aux = object1;
            object1 = object2;
            object2 = aux;
        }
    }
}
