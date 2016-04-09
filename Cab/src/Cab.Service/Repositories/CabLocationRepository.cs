using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Cab.Repositories
{
    public class CabLocationRepository : ICabLocationRepository
    {
        static readonly Dictionary<string, GeoCoordinate> CabLocationMap = new Dictionary<string, GeoCoordinate>()
            {
                { "1", new GeoCoordinate(12.9316556, 77.6226959) },
                { "2", new GeoCoordinate(12.8454649, 77.6721007) },
                { "3", new GeoCoordinate(12.99711, 77.61469) },
                { "4", new GeoCoordinate(12.965035, 77.5379688) },
                { "5", new GeoCoordinate(12.97576, 77.62283) }
            };
        public Dictionary<string, GeoCoordinate> GetLocationForCabs(IEnumerable<string> cabIds)
        {
            return CabLocationMap.Where(c => cabIds.Contains(c.Key)).ToDictionary(cabLocation => cabLocation.Key, cabLocation => cabLocation.Value);
        }

        public void SetCabLocation(string cabId, GeoCoordinate location)
        {
            if (CabLocationMap.ContainsKey(cabId))
            {
                CabLocationMap[cabId] = location;
            }
            else
            {
                CabLocationMap.Add(cabId, location);
            }
        }
    }
}
