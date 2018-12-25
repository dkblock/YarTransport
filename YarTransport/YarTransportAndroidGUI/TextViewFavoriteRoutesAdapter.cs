using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SearchWaySystem;

namespace YarTransportAndroidGUI
{
    class TextViewFavoriteRoutesAdapter : BaseAdapter<FavoriteRoutesNode>
    {
        private FavoriteRoutes _items;
        private Context _context;

        public TextViewFavoriteRoutesAdapter(Context context, FavoriteRoutes items)
        {
            _context = context;
            _items = items;
        }

        public override int Count
        {
            get { if (_items != null) return _items.FavoriteRoutesList.Count; else return -1; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override FavoriteRoutesNode this[int position]
        {
            get { return _items.FavoriteRoutesList[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
                row = LayoutInflater.FromContext(_context).Inflate(Resource.Layout.listview_row, null, false);
            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            txtName.Text = $"{_items.FavoriteRoutesList[position].PointOfDeparture} - {_items.FavoriteRoutesList[position].PointOfDestination}";
            return row;
        }
    }
}