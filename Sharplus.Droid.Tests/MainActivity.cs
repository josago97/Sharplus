using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Sharplus.Droid.Adapters;

namespace Sharplus.Droid.Tests
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _listView = FindViewById<ListView>(Resource.Id.listView);
            IEnumerable<Slot> slots = new[] { "Item1", "Item2", "Item3" }
                .Select(s => new StringSlot(s));
            _listView.Adapter = new ListAdapter(this, slots);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        class StringSlot : Slot<string>
        {
            public override int Layout => Resource.Layout.slot_layout;

            public StringSlot(string data) : base(data)
            {
            }

            public override void Show(View view, ViewGroup parent)
            {
                view.FindViewById<TextView>(Resource.Id.textView).Text = Data;
            }
        }
    }
}