using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Cab
{
    public class DistanceCalculator : IDistanceCalculator
    {
        public double GetDistance(GeoCoordinate location1, GeoCoordinate location2)
        {
            var dlt = location2.Latitude - location1.Latitude;
            var dln = location2.Longitude - location1.Longitude;
            return Math.Sqrt((dlt * dlt) + (dln * dln));
        }
    }
}
