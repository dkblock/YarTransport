using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchWaySystem
{
    [Serializable]
    public class FavoriteRoutes
    {
        public List<FavoriteRoutesNode> FavoriteRoutesList { get; private set; }

        public FavoriteRoutes()
        {
            FavoriteRoutesList = new List<FavoriteRoutesNode>();
        }

        public void Add(string routeName, string pointOfDeparture, string pointOfDestination)
        {
            FavoriteRoutesList.Add(new FavoriteRoutesNode(routeName, pointOfDeparture, pointOfDestination));
        }

        public void Remove(string routeName)
        {
            FavoriteRoutesList.RemoveAll(x => x.RouteName == routeName);
        }
    }

    [Serializable]
    public class FavoriteRoutesNode
    {
        public string RouteName { get; private set; }
        public string PointOfDeparture { get; private set; }
        public string PointOfDestination { get; private set; }

        public FavoriteRoutesNode(string routeName, string pointOfDeparture, string pointOfDestination)
        {
            RouteName = routeName;
            PointOfDeparture = pointOfDeparture;
            PointOfDestination = pointOfDestination;
        }
    }
}
