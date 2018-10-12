using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportLibrary
{
    [Serializable]
    public class AllStations
    {
        private Dictionary<Station, int> _stations;

        public AllStations()
        {
            _stations = new Dictionary<Station, int>();
        }

        public void AddStation(Station station)
        {
            var item = (from t in _stations.Keys where t.StationName == station.StationName select t).FirstOrDefault();

            if (item != null)
            {
                var newLocations = station.StationLocations.Except(item.StationLocations);

                foreach (var location in newLocations)
                    item.AddLocation(location);
            }
            else
                _stations.Add(station, _stations.Count);
        }

        public int Count
        {
            get { return _stations.Count; }
        }

        public int this[string stationName]
        {
            get
            {
                var item = (from t in _stations.Keys where t.StationName == stationName select t).FirstOrDefault();

                return _stations[item];
            }
            set
            {
                var item = (from t in _stations.Keys where t.StationName == stationName select t).FirstOrDefault();

                _stations[item] = value;
            }
        }

        public Station GetStation(int index)
        {
            return (from t in _stations where t.Value == index select t.Key).FirstOrDefault();
        }
    }
}
