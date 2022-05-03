namespace Sharplus.Architecture.Singleton
{
    public interface ISingleton<T>
    {
        public static T Instance { get; }
    }
}
