﻿using System;

namespace GMap
{
    public class Metrika
    {
        private double _eQuatorialEarthRadius = 6378.1370D;
        private double _d2r = (Math.PI / 180D);

        public int GetDistanceInM(double lat1, double long1, double lat2, double long2)
        {
            return (int)(1000D * GetDistanceInKM(lat1, long1, lat2, long2));
        }

        private double GetDistanceInKM(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2D), 2D) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }
    }
}
