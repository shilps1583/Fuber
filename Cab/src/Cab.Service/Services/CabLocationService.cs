using System.Collections.Generic;
using System.Linq;
using Cab.Repositories;
using Domain;

namespace Cab.Services
{
    public class CabLocationService : ICabLocationService
    {
        private readonly IDistanceCalculator DistanceCalculator;
        private readonly ICabLocationRepository CabLocationRepository;

        public CabLocationService(IDistanceCalculator distanceCalculator, ICabLocationRepository cabLocationRepository)
        {
            DistanceCalculator = distanceCalculator;
            CabLocationRepository = cabLocationRepository;
        }
        public Dictionary<string, GeoCoordinate> GetCabsWithinDistance(int withinDistance, GeoCoordinate location, IEnumerable<string> cabIds)
        {
            var cabsWithLocation = CabLocationRepository.GetLocationForCabs(cabIds);
            var cabsWithDistance = new Dictionary<KeyValuePair<string,GeoCoordinate>,double>();
            foreach (var cabLocation in cabsWithLocation)
            {
                var distance = DistanceCalculator.GetDistance(location, cabLocation.Value);
                if (distance <= withinDistance)
                {
                    cabsWithDistance.Add(cabLocation, distance);
                }
            }
            return cabsWithDistance.OrderBy(c => c.Value).ToDictionary(val => val.Key.Key, val => val.Key.Value);
        }

        public void SetCabLocation(string cabId, GeoCoordinate location)
        {
            CabLocationRepository.SetCabLocation(cabId,location);
        }

        public Dictionary<string, GeoCoordinate> GetLocationForCabs(IEnumerable<string> cabIds)
        {
            return CabLocationRepository.GetLocationForCabs(cabIds);
        }
    }
}
