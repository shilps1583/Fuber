using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Cab.Repositories
{
    public class AvailableCabsRepository : IAvailableCabsRepository
    {
        private static readonly List<Domain.Cab> AvailableCabs = new List<Domain.Cab>(CabsDatabase.GetAllCabs());

        public List<Domain.Cab> GetAvailableCabs()
        {
            return AvailableCabs;
        }

        public void AddCabToPool(Domain.Cab cab)
        {
            AvailableCabs.Add(cab);
        }

        public void AddCabToPool(string cabId)
        {
            AvailableCabs.Add(CabsDatabase.GetAllCabs().FirstOrDefault(c => c.Id.Equals(cabId)));
        }

        public void RemoveCabFromPool(Domain.Cab cab)
        {
            AvailableCabs.Remove(cab);
        }
    }
}
