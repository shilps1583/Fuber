using System.Collections.Generic;

namespace Cab.Repositories
{
    public interface IAvailableCabsRepository
    {
        List<Domain.Cab> GetAvailableCabs();
        void AddCabToPool(Domain.Cab cab);
        void AddCabToPool(string cabId);
        void RemoveCabFromPool(Domain.Cab cab);
    }
}
