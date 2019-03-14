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
using TransportLibrary;
using Android.Views.InputMethods;

namespace YarTransportAndroidGUI
{
    [Activity(Icon="@drawable/SplashScreen",Theme = "@style/SplashScreen", MainLauncher = true)]
    public class MainActivity :AppCompatActivity,IDialogInterfaceOnClickListener,IDialogInterfaceOnMultiChoiceClickListener
    {
        ListView text;
        List<SearchWaySystem.RouteInfo> list;
        string firstStation, secondStation;
        ProgressDialog progressDialog;
        CustomTextView startSpinner, endSpinner;
        ImageButton favouritiesBtn;
        bool[] tral = new bool[] { true, true, true, true };
        string[] name = new string[] { "Троллейбус", "Автобус", "Трамвай", "Маршрутка" };
        List<string> items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Что приедет";

            favouritiesBtn = FindViewById<ImageButton>(Resource.Id.FavouritiesButton);
            favouritiesBtn.Click += Fbtn_Click;

            SearcherAdapter.DownloadDataBase(Assets.Open("allroutes.dat"), Assets.Open("allstations.dat"), Assets.Open("routematrix.dat"));
            SearcherAdapter.Init();

            AllStations stations = SearcherAdapter.GetAllStations();
            items = new List<string>();

            for (int i = 0; i < stations.Count; i++)
            {
                items.Add(stations.GetStation(i).StationName.ToString());
            }

            startSpinner =new CustomTextView(FindViewById<AutoCompleteTextView>(Resource.Id.Start), new ArrayAdapter(this, Resource.Layout.list_item, items));
            startSpinner.textView.ItemClick += _ItemClick;
            endSpinner = new CustomTextView(FindViewById<AutoCompleteTextView>(Resource.Id.End), new ArrayAdapter(this, Resource.Layout.list_item, items));
            endSpinner.textView.ItemClick += _ItemClick;

            text = FindViewById<ListView>(Resource.Id.Schedule);
            text.ItemClick += Text_ItemClick;
            var btn = FindViewById<Button>(Resource.Id.SearchButton);
            btn.Click += FindTransport;

            if (SearcherAdapter.St != null)
            {
                var m = SearcherAdapter.GetPoint();
                startSpinner.Text=items[m[0]];
                endSpinner.Text=items[m[1]];
                favouritiesBtn.SetImageResource(Resource.Drawable.Contains);
            }

            FindViewById<LinearLayout>(Resource.Id.rootLayout).RequestFocus();
        }

        private void _ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            FindViewById<LinearLayout>(Resource.Id.rootLayout).RequestFocus();
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.ToggleSoftInput(InputMethodManager.ShowForced, 0);

            if (SearcherAdapter.CheckRoute(startSpinner.Text, endSpinner.Text))
                favouritiesBtn.SetImageResource(Resource.Drawable.Contains);
            else           
                favouritiesBtn.SetImageResource(Resource.Drawable.NotContains);          
        }

        private void Fbtn_Click(object sender, System.EventArgs e)
        {
           if (items.Contains(startSpinner.Text)&&items.Contains(endSpinner.Text))
            {
                firstStation = startSpinner.Text;
                secondStation = endSpinner.Text;

                if (!SearcherAdapter.CheckRoute(firstStation, secondStation))
                {
                    SearcherAdapter.AddFavoriteRoute(new SearchWaySystem.FavoriteRoutesNode(firstStation+"-"+secondStation,firstStation,secondStation));
                    favouritiesBtn.SetImageResource(Resource.Drawable.Contains);
                    Toast.MakeText(this,"Маршрут добавлен в избранное", ToastLength.Short).Show();
                }
                else
                {
                    SearcherAdapter.DeleteFavoriteRoute(firstStation, secondStation);
                    favouritiesBtn.SetImageResource(Resource.Drawable.NotContains);
                    Toast.MakeText(this, "Маршрут удален из избранного", ToastLength.Short).Show();
                }
                SearcherAdapter.SaveFavoriteRoutes();
            }
        }

        private void FindTransport(object sender, System.EventArgs e)
        {
            firstStation = startSpinner.Text;
            secondStation = endSpinner.Text;
            bool isSelected;

            if (items.Contains(startSpinner.Text) && items.Contains(endSpinner.Text))
                isSelected = true;
            else
                isSelected = false;


            var cm = ((ConnectivityManager)GetSystemService(Application.ConnectivityService)).ActiveNetworkInfo;
            bool isConnected;

            if (cm == null)
                isConnected = false;
            else
                isConnected = cm.IsConnected;

            if (isConnected && isSelected)
                ShowInfoAsync();
            else
                if (!isConnected)
                Toast.MakeText(this, "Нет подключения к интернету", ToastLength.Short).Show();
            else
                Toast.MakeText(this, "Чтобы начать поиск, выберите остановки", ToastLength.Short).Show();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public void GetRoutesAsync()
        {
            if (list == null)
                list = new List<SearchWaySystem.RouteInfo>();

            list = SearcherAdapter.GetRoutes(firstStation, secondStation,tral[0],tral[1], tral[2], tral[3]);
        }

        public async void ShowInfoAsync()
        {
            text.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new List<string>() { "Подождите, идет загрузка" });
            await Task.Run(() => GetRoutesAsync());
            var adapter = new TextViewRouteInfoAdapter(this, list);
            text.Adapter = adapter;
            if (list==null||list.Count==0)
                text.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new List<string>() { "Не существует транспорта, следующего по заданному маршруту либо транспорт ещё не вышел на маршрут" });
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