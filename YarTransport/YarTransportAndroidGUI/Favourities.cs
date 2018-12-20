using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace YarTransportAndroidGUI
{
    [Activity(Label = "Favourities", Theme = "@style/AppTheme")]
    public class Favourities : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_favourities);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar2);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.Title = "Избранное";
            var text = FindViewById<ListView>(Resource.Id.FavouritiesList);
            var adapter = new TextViewFavoriteRoutesAdapter(this, SearcherAdapter.GetFavoriteRoutes());
            text.Adapter = adapter;
            text.ItemClick += Text_ItemClick;
        }

        private void Text_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            SearcherAdapter.SetPoint(SearcherAdapter.GetFavoriteRoutes().FavoriteRoutesList[e.Position].PointOfDeparture, SearcherAdapter.GetFavoriteRoutes().FavoriteRoutesList[e.Position].PointOfDestination);
            StartActivity(typeof(MainActivity));
            OverridePendingTransition(Resource.Animation.slide_left, Resource.Animation.fade_out);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home)
                Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}