using Android.Views;

namespace Sharplus.Droid.Adapters
{
    public abstract class Slot
    {
        public object Data { get; private set; }
        public abstract int Layout { get; }

        public Slot(object data)
        {
            Data = data;
        }

        public abstract void Show(View view, ViewGroup parent);
    }

    public abstract class Slot<T> : Slot
    {
        public new T Data => (T)base.Data;

        public Slot(T data) : base(data) { }
    }
}