using System.Collections.Generic;
using Domain;

namespace Cab
{
    public interface IDistanceCalculator
    {
        double GetDistance(GeoCoordinate location1, GeoCoordinate location2);
    }
}
