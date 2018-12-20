using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Views;
using System.Collections.Generic;
using SearchableSpinner.Droid.Controls;
using System.Threading.Tasks;
using Android.Widget;
using Android.Graphics;
using Java.Lang;
using Android.Net;

namespace YarTransportAndroidGUI
{
    [Activity(Icon="@drawable/SplashScreen",Theme = "@style/SplashScreen", MainLauncher = true)]
    public class MainActivity :AppCompatActivity,IDialogInterfaceOnClickListener,IDialogInterfaceOnMultiChoiceClickListener
    {
        ListView text;
        List<SearchWaySystem.RouteInfo> list;
        string s, s1;
        ProgressDialog progressDialog;
        SpinnerSearch startSpinner, endSpinner;
        bool[] tral = new bool[] { true, true, true, true };
        string[] name = new string[] { "Троллейбус", "Автобус", "Трамвай", "Маршрутка" };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Что приедет";
            var fbtn = FindViewById<ImageButton>(Resource.Id.FavouritiesButton);
            fbtn.Click += Fbtn_Click;
            SearcherAdapter.DownloadDataBase(Assets.Open("allroutes.dat"), Assets.Open("allstations.dat"), Assets.Open("routematrix.dat"));
            SearcherAdapter.Init();
            var stations = SearcherAdapter.GetAllStations();
            List<SpinnerItem> items = new List<SpinnerItem>();

            for (int i = 0; i < stations.Count; i++)
            {
                items.Add(new SpinnerItem { Id = i, Name = stations.GetStation(i).StationName });
            }

            startSpinner = FindViewById<SpinnerSearch>(Resource.Id.StartSpinner);
            startSpinner.DefaultText = "Откуда";

            startSpinner.ItemSelected += Spinner_ItemSelected;
            endSpinner = FindViewById<SpinnerSearch>(Resource.Id.EndSpinner);
            endSpinner.DefaultText = "Куда";           
            endSpinner.ItemSelected +=Spinner_ItemSelected;
            
            if (SearcherAdapter.St != null)
            {
                var m = SearcherAdapter.GetPoint();
                if (m[1] < m[0])
                {
                    startSpinner.SetItems(items, m[0], null);
                    endSpinner.SetItems(items, m[1], null);
                }
                else
                {
                    endSpinner.SetItems(items, m[1], null);
                    startSpinner.SetItems(items, m[0], null);                    
                }
            }
            else
            {
                startSpinner.SetItems(items, -1, null);
                endSpinner.SetItems(items, -1, null);
            }
            

            text = FindViewById<ListView>(Resource.Id.Schedule);
            text.ItemClick += Text_ItemClick;
            var btn = FindViewById<Button>(Resource.Id.SearchButton);
            btn.Click += FindTransport;

        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
           if (startSpinner.SelectedItemId>=0&&endSpinner.SelectedItemId>=0&&SearcherAdapter.CheckRoute(startSpinner.SelectedItem.ToString(), endSpinner.SelectedItem.ToString()))
            {
                var btn = FindViewById<ImageButton>(Resource.Id.FavouritiesButton);
                btn.SetImageResource(Resource.Drawable.Contains);
            }
           else
            {
                var btn = FindViewById<ImageButton>(Resource.Id.FavouritiesButton);
                btn.SetImageResource(Resource.Drawable.NotContains);
            }
        }

        private void Fbtn_Click(object sender, System.EventArgs e)
        {
           if (startSpinner.SelectedItemId >= 0 && endSpinner.SelectedItemId >= 0)
            {
                s = FindViewById<SpinnerSearch>(Resource.Id.StartSpinner).SelectedItem.ToString();
                s1 = FindViewById<SpinnerSearch>(Resource.Id.EndSpinner).SelectedItem.ToString();
                if (!SearcherAdapter.CheckRoute(s, s1))
                {
                    SearcherAdapter.AddFavoriteRoute(new SearchWaySystem.FavoriteRoutesNode(s+"-"+s1,s,s1));
                    var btn = FindViewById<ImageButton>(Resource.Id.FavouritiesButton);
                    btn.SetImageResource(Resource.Drawable.Contains);
                    Toast.MakeText(this,"Маршрут добавлен в избранное", ToastLength.Short).Show();
                }
                else
                {
                    SearcherAdapter.DeleteFavoriteRoute(s, s1);
                    var btn = FindViewById<ImageButton>(Resource.Id.FavouritiesButton);
                    btn.SetImageResource(Resource.Drawable.NotContains);
                    Toast.MakeText(this, "Маршрут удален из избранного", ToastLength.Short).Show();
                }
                SearcherAdapter.SaveFavoriteRoutes();
            }
        }

        private void FindTransport(object sender, System.EventArgs e)
        {
            s = FindViewById<SpinnerSearch>(Resource.Id.StartSpinner).SelectedItem.ToString();
            s1 = FindViewById<SpinnerSearch>(Resource.Id.EndSpinner).SelectedItem.ToString();
            var cm = ((ConnectivityManager)GetSystemService(Application.ConnectivityService)).ActiveNetworkInfo;
            bool isConnected;
            if (cm == null)
                isConnected = false;
            else
                isConnected= cm.IsConnected;
            if (isConnected)
            ShowInfoAsync();
            else
            {
                Toast.MakeText(this, "Нет подключения к интернету", ToastLength.Short).Show();
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public void GetRoutesAsync()
        {
            if (list == null)
                list = new List<SearchWaySystem.RouteInfo>();
            list = SearcherAdapter.GetRoutes(s, s1,tral[0],tral[1], tral[2], tral[3]);
        }

        public async void ShowInfoAsync()
        {
            text.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new List<string>() { "Подождите, идет загрузка" });
            await Task.Run(() => GetRoutesAsync());
            var adapter = new TextViewRouteInfoAdapter(this, list);
            text.Adapter = adapter;
        }

        public string GetInfo(int position)
        {
             string info = "";
            if (list[position].TransportModel != "Unknown")
            {
                info += $"{list[position].TransportModel}\n\n";
                foreach (var node in list[position].Schedule)
                    info+=$"{node.ToString()}\n";
            }
            else
            {
                info+=$"\n{list[position].Schedule[0].ToString()}\n\n";
                info += $"Подробная информация о маршруте будет доступна по прибытии транспорта на конечную остановку";
            }
            return info;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Layout.top_menus, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.menu_favourities)
            {
                StartActivity(typeof(Favourities));
                OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.fade_out);
            }
            else
            {
                ShowDialog(0);              
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch(id)
            {
                case 0:
                    {
                        return new Android.Support.V7.App.AlertDialog.Builder(this)
                            .SetTitle("Фильтры")
                            .SetPositiveButton("Oк",this)
                            .SetNegativeButton("Назад",this)
                            .SetMultiChoiceItems(name,tral,this)
                            .Create();
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        public void OnClick(IDialogInterface dialog, int which)//buttons
        {
        }

        public void OnClick(IDialogInterface dialog, int which, bool isChecked)//checkBox
        {
            tral[which] = isChecked;
        }

        private void Text_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(TransportInfo));
            intent.PutExtra("info", GetInfo(e.Position));
            intent.PutExtra("infoT", list[e.Position].RouteType);
            
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.fade_out);
        }
    }
}