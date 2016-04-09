using System.Collections.Generic;
using System.Linq;
using Cab.Repositories;
using Domain;

namespace Cab.Services
{
    public class AvailableCabsService : IAvailableCabsService
    {
        private readonly ICabLocationService CabLocationService;
        private readonly IAvailableCabsRepository AvailableCabsRepository;
        

        public AvailableCabsService(ICabLocationService cabLocationService, 
            IAvailableCabsRepository availableCabsRepository)
        {
            CabLocationService = cabLocationService;
            AvailableCabsRepository = availableCabsRepository;
        }

        public Domain.Cab GetNearestCab(GeoCoordinate location, CabType[] cabTypes)
        {
            var availableCabs = AvailableCabsRepository.GetAvailableCabs().Where(c => cabTypes.Contains(c.Type));
            if (availableCabs.Any())
            {
                var cabsWithLocation = CabLocationService.GetCabsWithinDistance(3, location, availableCabs.Select(a => a.Id));
                
                if (cabsWithLocation.Any())
                {
                    var nearestCabId = cabsWithLocation.FirstOrDefault().Key;
                    var nearestCab = availableCabs.FirstOrDefault(c => c.Id.Equals(nearestCabId));
                    AvailableCabsRepository.RemoveCabFromPool(nearestCab);
                    return nearestCab;
                }
            }
            return null;
        }

        public void ReturnCabToPool(Domain.Cab cab)
        {
            AvailableCabsRepository.AddCabToPool(cab);
        }

        public void ReturnCabToPool(string cabId)
        {
            AvailableCabsRepository.AddCabToPool(cabId);
        }

        public IEnumerable<Domain.Cab> GetAvailableCabDetails()
        {
            return AvailableCabsRepository.GetAvailableCabs();
        }
    }
}
