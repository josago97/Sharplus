using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.SwipeRefreshLayout.Widget;

namespace Sharplus.Droid.Widgets
{
    public class SwipeRefreshLayoutAllList : SwipeRefreshLayout
    {
        private ListView[] _allListView;

        public SwipeRefreshLayoutAllList(Context context) : base(context)
        {
        }

        public SwipeRefreshLayoutAllList(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected SwipeRefreshLayoutAllList(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();
            GetAllListView();
        }

        private void GetAllListView()
        {
            _allListView = GetListViews(this).ToArray();
        }

        private ListView[] GetListViews(ViewGroup viewGroup)
        {
            List<ListView> listViews = new List<ListView>();

            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                ViewGroup child = viewGroup.GetChildAt(i) as ViewGroup;

                if (child != null)
                {
                    if (child is ListView listView)
                    {
                        listViews.Add(listView);
                    }

                    listViews.AddRange(GetListViews(child));
                }
            }

            return listViews.ToArray();
        }

        public override bool CanChildScrollUp()
        {
            bool isAllFirstElementVisible = true;

            if (_allListView.Length > 0)
            {
                isAllFirstElementVisible = _allListView.All(l =>
                {
                    int firstElementVisiblePosition = l.FirstVisiblePosition;

                    if (l.ChildCount > 0 && l.GetChildAt(0).Top < 0)
                    {
                        firstElementVisiblePosition = l.FirstVisiblePosition + 1;
                    }

                    return firstElementVisiblePosition == 0;
                });
            }

            return base.CanChildScrollUp() || !isAllFirstElementVisible;
        }
    }
}