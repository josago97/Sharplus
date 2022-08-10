using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Sharplus.Droid.Adapters
{
    public class ListAdapter : BaseAdapter<object>
    {
        private Context _context;
        private Dictionary<int, int> _layoutTypes;
        private int _viewTypeCount;

        public IList<Slot> Slots { get; private set; }
        public override int Count => Slots.Count;
        public override object this[int position] => Slots[position].Data;
        public override int ViewTypeCount => _viewTypeCount;

        public ListAdapter(Context context, IEnumerable<Slot> slots) : base()
        {
            _context = context;

            SetData(slots);
        }

        public ListAdapter(Context context, int viewTypeCount = 1) : this(context, new Slot[0])
        {
            _viewTypeCount = viewTypeCount;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int GetItemViewType(int position)
        {
            int layout = Slots[position].Layout;

            return _layoutTypes[layout];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Slot slot = Slots[position];

            if (convertView == null)
            {
                int layout = slot.Layout;
                convertView = LayoutInflater.From(_context).Inflate(layout, null);
            }

            slot.Show(convertView, parent);

            return convertView;
        }

        public virtual void Update()
        {
            NotifyDataSetChanged();
        }

        public virtual void Update(IEnumerable<Slot> slots)
        {
            SetData(slots);
            Update();
        }

        private void SetData(IEnumerable<Slot> slots)
        {
            Slots = slots.ToArray();
            _layoutTypes = Slots.GroupBy(s => s.Layout)
                .Select((g, i) => (g.Key, i))
                .ToDictionary(x => x.Key, x => x.i);

            _viewTypeCount = Math.Max(_layoutTypes.Count, 1);
        }
    }

    public class ListAdapter<T> : ListAdapter
    {
        public new T this[int position] => (T)base[position];
        public new IList<Slot<T>> Slots => (IList<Slot<T>>)base.Slots;

        public ListAdapter(Context context, IEnumerable<Slot<T>> slots) : base(context, slots)
        {
        }

        public ListAdapter(Context context) : this(context, new List<Slot<T>>())
        {
        }
    }
}