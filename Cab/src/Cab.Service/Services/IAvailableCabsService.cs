using System.Collections.Generic;
using Domain;

namespace Cab.Services
{
    public interface IAvailableCabsService
    {
        Domain.Cab GetNearestCab(GeoCoordinate location, CabType[] cabTypes);
        void ReturnCabToPool(Domain.Cab cab);
        void ReturnCabToPool(string cabId);
        IEnumerable<Domain.Cab> GetAvailableCabDetails();
    }
}