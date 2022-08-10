using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Sharplus.Droid.Adapters
{
    public class RecyclerViewAdapter : RecyclerView.Adapter
    {
        public IList<Slot> Slots { get; private set; }
        public override int ItemCount => Slots.Count;

        public RecyclerViewAdapter(IEnumerable<Slot> slots) : base()
        {
            SetData(slots);
        }

        public RecyclerViewAdapter() : this(new Slot[0]) { }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Slots[position].Show(holder.ItemView, ((RecyclerViewHolder)holder).Parent);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Slots.First().Layout, parent, false);

            return new RecyclerViewHolder(itemView, parent);
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
        }

        private class RecyclerViewHolder : RecyclerView.ViewHolder
        {
            public ViewGroup Parent { get; private set; }

            public RecyclerViewHolder(View itemView, ViewGroup parent) : base(itemView)
            {
                Parent = parent;
            }
        }
    }

    public class RecyclerViewAdapter<T> : RecyclerViewAdapter
    {
        public new IList<Slot<T>> Slots => (IList<Slot<T>>)base.Slots;

        public RecyclerViewAdapter(IEnumerable<Slot<T>> slots) : base(slots)
        {
        }

        public RecyclerViewAdapter() : this(new List<Slot<T>>())
        {
        }
    }

    public class RecyclerViewAdapter<T, G> : RecyclerViewAdapter<T> where G : Slot<T>
    {
        public RecyclerViewAdapter(IEnumerable<T> data)
            : base(data.Select(t => (G)Activator.CreateInstance(typeof(G), t)))
        {
        }
    }
}
