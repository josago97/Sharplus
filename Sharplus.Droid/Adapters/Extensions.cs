using Android.Widget;

namespace Sharplus.Droid.Adapters
{
    public static class Extensions
    {
        public static ListAdapter<T> ToListAdapter<T>(this IListAdapter adapter)
        {
            return (ListAdapter<T>)adapter;
        }

        public static ListAdapter<T> GetListAdapter<T>(this AbsListView listView)
        {
            return listView.Adapter.ToListAdapter<T>();
        }
    }
}