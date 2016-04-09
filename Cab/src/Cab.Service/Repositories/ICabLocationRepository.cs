using System.Collections.Generic;
using Domain;

namespace Cab.Repositories
{
    public interface ICabLocationRepository
    {
        Dictionary<string, GeoCoordinate> GetLocationForCabs(IEnumerable<string> cabIds);
        void SetCabLocation(string cabId, GeoCoordinate location);
    }
}
