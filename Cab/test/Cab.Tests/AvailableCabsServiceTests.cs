using System.Collections.Generic;
using System.Linq;
using Cab.Repositories;
using Domain;
using Moq;
using NUnit.Framework;
using Cab.Services;

namespace Cab.Tests
{
    [TestFixture]
    public class AvailableCabsServiceTests
    {
        [Test]
        public void GetNearestCabWhenCabIsAvailable()
        {
            var allCabTypes = new CabType[] {CabType.Regular, CabType.Pink};
            var allCabs = new List<Domain.Cab>
            {
                new Domain.Cab("1", "Toyota Etios", CabType.Regular),
                new Domain.Cab("2", "Maruti Swift", CabType.Regular),
                new Domain.Cab("3", "Toyota Etios", CabType.Pink),
                new Domain.Cab("4", "Maruti Swift", CabType.Regular),
                new Domain.Cab("5", "Toyota Etios", CabType.Pink),
            };
            var cabLocationMap = new Dictionary<string, GeoCoordinate>()
            {
                { "3", new GeoCoordinate(12.99711, 77.61469) },
                { "4", new GeoCoordinate(12.97576, 77.62283) }
            };
            var pickupLocation = new GeoCoordinate(13.0085, 77.59087);
            var cabLocationService = new Mock<ICabLocationService>();
            cabLocationService.Setup(
                c => c.GetCabsWithinDistance(
                    It.IsAny<int>(), It.IsAny<GeoCoordinate>(), It.IsAny<IEnumerable<string>>()))
                    .Returns((int i, GeoCoordinate g, IEnumerable<string> l) =>
                    {
                        return cabLocationMap.
                            Where(cl => l.Contains(cl.Key)).
                            ToDictionary(cl => cl.Key, cl => cl.Value);
                    });
            var availableCabsRepo = new Mock<IAvailableCabsRepository>();
            availableCabsRepo.Setup(a => a.GetAvailableCabs()).Returns(allCabs);

            var availableCabsService = new AvailableCabsService(cabLocationService.Object, availableCabsRepo.Object);

            var expectedCab = allCabs[2];
            var availableCab = availableCabsService.GetNearestCab(pickupLocation, allCabTypes);

            Assert.That(expectedCab.Equals(availableCab));
            availableCabsRepo.Verify(a => a.RemoveCabFromPool(availableCab));
        }

        [Test]
        public void GetNullWhenCabIsNotAvailable()
        {
            var allCabTypes = new CabType[] { CabType.Regular, CabType.Pink };
            var pickupLocation = new GeoCoordinate(13.0085, 77.59087);
            var cabLocationService = new Mock<ICabLocationService>();
            
            var availableCabsRepo = new Mock<IAvailableCabsRepository>();
            availableCabsRepo.Setup(a => a.GetAvailableCabs()).Returns(new List<Domain.Cab>());

            var availableCabsService = new AvailableCabsService(cabLocationService.Object, availableCabsRepo.Object);

            var availableCab = availableCabsService.GetNearestCab(pickupLocation,allCabTypes);

            Assert.IsNull(availableCab);
        }

        [Test]
        public void GetNullWhenCabIsTooFarAway()
        {
            var allCabTypes = new CabType[] { CabType.Regular, CabType.Pink };
            var allCabs = new List<Domain.Cab>
            {
                new Domain.Cab("1", "Toyota Etios", CabType.Regular),
                new Domain.Cab("2", "Maruti Swift", CabType.Regular),
                new Domain.Cab("3", "Toyota Etios", CabType.Pink),
                new Domain.Cab("4", "Maruti Swift", CabType.Regular),
                new Domain.Cab("5", "Toyota Etios", CabType.Pink),
            };
            var cabLocationMap = new Dictionary<string, GeoCoordinate>();
            var pickupLocation = new GeoCoordinate(13.0620326, 77.5977596);
            var cabLocationService = new Mock<ICabLocationService>();
            cabLocationService.Setup(
                c => c.GetCabsWithinDistance(It.IsAny<int>(), It.IsAny<GeoCoordinate>(), It.IsAny<IEnumerable<string>>()))
                .Returns(cabLocationMap);
            var availableCabsRepo = new Mock<IAvailableCabsRepository>();
            availableCabsRepo.Setup(a => a.GetAvailableCabs()).Returns(allCabs);

            var availableCabsService = new AvailableCabsService(cabLocationService.Object, availableCabsRepo.Object);

            var availableCab = availableCabsService.GetNearestCab(pickupLocation,allCabTypes);

            Assert.IsNull(availableCab);
        }

        [Test]
        public void ReturnCabToPoolShouldCallAddCabToPool()
        {
            var cab = new Domain.Cab("1", "Toyota", CabType.Regular);
            var cabLocationService = new Mock<ICabLocationService>();
            var availableCabsRepo = new Mock<IAvailableCabsRepository>();

            var availableCabsService = new AvailableCabsService(cabLocationService.Object, availableCabsRepo.Object);

            availableCabsService.ReturnCabToPool(cab);

            availableCabsRepo.Verify(a => a.AddCabToPool(cab));
        }

        [Test]
        public void ReturnCabToPoolByIdShouldCallAddCabToPool()
        {
            var cabLocationService = new Mock<ICabLocationService>();
            var availableCabsRepo = new Mock<IAvailableCabsRepository>();

            var availableCabsService = new AvailableCabsService(cabLocationService.Object, availableCabsRepo.Object);

            availableCabsService.ReturnCabToPool("1");

            availableCabsRepo.Verify(a => a.AddCabToPool("1"));
        }

        [Test]
        public void GetCabsFilteredByCabType()
        {
            var allCabTypes = new CabType[] { CabType.Pink };
            var allCabs = new List<Domain.Cab>
            {
                new Domain.Cab("1", "Toyota Etios", CabType.Regular),
                new Domain.Cab("2", "Maruti Swift", CabType.Regular),
                new Domain.Cab("3", "Toyota Etios", CabType.Regular),
                new Domain.Cab("4", "Maruti Swift", CabType.Pink),
                new Domain.Cab("5", "Toyota Etios", CabType.Pink),
            };
            var cabLocationMap = new Dictionary<string, GeoCoordinate>()
            {
                { "3", new GeoCoordinate(12.99711, 77.61469) },
                { "4", new GeoCoordinate(12.97576, 77.62283) }
            };
            var pickupLocation = new GeoCoordinate(13.0085, 77.59087);
            var cabLocationService = new Mock<ICabLocationService>();
            cabLocationService.Setup(
                c => c.GetCabsWithinDistance(
                    It.IsAny<int>(), It.IsAny<GeoCoordinate>(), It.IsAny<IEnumerable<string>>()))
                    .Returns((int i, GeoCoordinate g, IEnumerable<string> l) =>
                    {
                        return cabLocationMap.
                            Where(cl => l.Contains(cl.Key)).
                            ToDictionary(cl => cl.Key, cl => cl.Value);
                    });
            var availableCabsRepo = new Mock<IAvailableCabsRepository>();
            availableCabsRepo.Setup(a => a.GetAvailableCabs()).Returns(allCabs);

            var availableCabsService = new AvailableCabsService(cabLocationService.Object, availableCabsRepo.Object);

            var expectedCab = allCabs[3];
            var availableCab = availableCabsService.GetNearestCab(pickupLocation, allCabTypes);

            Assert.That(expectedCab.Equals(availableCab));
            availableCabsRepo.Verify(a => a.RemoveCabFromPool(availableCab));
        }
    }
}
