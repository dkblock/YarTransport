using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SearchWaySystem;
using TransportLibrary;

namespace YarTransportAndroidGUI
{
    public static class SearcherAdapter
    {
        private static Searcher _searcher;
        private static AllRoutes _allRoutes;
        private static AllStations _allStations;
        private static RouteMatrix _routeMatrix;
        private static FavoriteRoutes _favoriteRoutes;

        public static string St;
        public static string En;

        public static void DownloadDataBase(Stream AllRoutesStream, Stream AllStationsStream, Stream RouteMatrixStream)
        {
            if (_allStations == null)
            {
                var formatter = new BinaryFormatter();
                _allRoutes = (AllRoutes)formatter.Deserialize(AllRoutesStream);
                _allStations = (AllStations)formatter.Deserialize(AllStationsStream);
                _routeMatrix = (RouteMatrix)formatter.Deserialize(RouteMatrixStream);


                var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "favorites.dat");

                if (backingFile == null || !File.Exists(backingFile))
                {
                    return;
                }
                using (var reader = new FileStream(backingFile,FileMode.OpenOrCreate))
                {
                    _favoriteRoutes = new FavoriteRoutes();
                    _favoriteRoutes = (FavoriteRoutes)formatter.Deserialize(reader);
                }

                
            }
        }

        public static void Init()
        {
            if(_searcher==null)
            _searcher = new Searcher(_allRoutes, _allStations, _routeMatrix);
        }

        public static AllStations GetAllStations()
        {
            return _allStations;
        }

        public static List<RouteInfo> GetRoutes(string pointOfDeparture, string pointOfDestination,bool a,bool b,bool c,bool d)
        {
            return _searcher.GetRoutes(pointOfDeparture, pointOfDestination,b,a,c,d);
        }

        public static FavoriteRoutes GetFavoriteRoutes()
        {
            return _favoriteRoutes;
        }

        public static bool CheckRoute(string pointOfDeparture, string pointOfDestination)
        {
            if (_favoriteRoutes == null)
                return false;
            else
            return _favoriteRoutes.FavoriteRoutesList.Any(x => x.PointOfDeparture.Equals(pointOfDeparture) && x.PointOfDestination.Equals(pointOfDestination));
        }

        public static void AddFavoriteRoute(FavoriteRoutesNode node)
        {
            if (_favoriteRoutes == null)
                _favoriteRoutes = new FavoriteRoutes();
            _favoriteRoutes.Add(node.RouteName,node.PointOfDeparture,node.PointOfDestination);
        }

        public static void DeleteFavoriteRoute(string pointOfDeparture,string pointOfDestination)
        {
            _favoriteRoutes.Remove(_favoriteRoutes.FavoriteRoutesList.Find(x => x.PointOfDeparture.Equals(pointOfDeparture) && x.PointOfDestination.Equals(pointOfDestination)).RouteName);
        }

        public static  void SetPoint(string st,string en)
        {
            St = st;
            En = en;
        }

        public static List<int> GetPoint()
        {
            List<int> vs = new List<int>() { -1, -1 };
            if (_favoriteRoutes != null)
            {              
                for (int i = 0; i < _allStations.Count; i++)
                {
                    if (_allStations.GetStation(i).StationName.Equals(St))
                        vs[0] = i;
                    if (_allStations.GetStation(i).StationName.Equals(En))
                        vs[1] = i;
                }
            }
            if (vs[0]==-1||vs[1]==-1)
                vs = new List<int>() { -1, -1 };
            St = null;
            En = null;
            return vs;
        }
        public static void SaveFavoriteRoutes()
        {
          var formatter = new BinaryFormatter();
           var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "favorites.dat");
            using (var reader = new FileStream(backingFile, FileMode.OpenOrCreate))
            {
                formatter.Serialize(reader, _favoriteRoutes);
            }
            
        }
    }
}