using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportLibrary
{
    [Serializable]
    public class AllRoutes
    {
        private Dictionary<Route, RouteContent> _allRoutes;

        public AllRoutes()
        {
            _allRoutes = new Dictionary<Route, RouteContent>();
        }

        public void AddRoute(Route route, RouteContent routeContent)
        {
            _allRoutes.Add(route, routeContent);
        }

        public int Count
        {
            get { return _allRoutes.Count; }
        }

        public RouteContent this[Route route]
        {
            get
            {
                var item = (from t in _allRoutes.Keys where t.ToString() == route.ToString() select t).FirstOrDefault();

                return _allRoutes[item];
            }
            set
            {
                var item = (from t in _allRoutes.Keys where t == route select t).FirstOrDefault();

                _allRoutes[item] = value;
            }
        }
    }
}
