namespace System
{
    public static class ObjectPlus
    {
        public static void Swap<T>(ref T object1, ref T object2)
        {
            T aux = object1;
            object1 = object2;
            object2 = aux;
        }
    }
}
