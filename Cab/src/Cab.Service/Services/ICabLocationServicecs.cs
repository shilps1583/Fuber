using System.Collections.Generic;
using Domain;

namespace Cab.Services
{
    public interface ICabLocationService
    {
        Dictionary<string, GeoCoordinate> GetCabsWithinDistance(int distance, GeoCoordinate location, IEnumerable<string> cabIds );
        void SetCabLocation(string cabId, GeoCoordinate location);
        Dictionary<string, GeoCoordinate> GetLocationForCabs(IEnumerable<string> cabIds);
    }
}
