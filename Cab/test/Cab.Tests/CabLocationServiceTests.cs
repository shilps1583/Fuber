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
    public class CabLocationServiceTests
    {
        [Test]
        public void ShouldReturnCabsWithinSpecifiedDistanceOfLocationSortedByDistance()
        {
            var location = new GeoCoordinate(12,77);
            var allCabs = new List<string>
            {
                "1", "2", "3", "4", "5"
            };
            var coordinates = new List<GeoCoordinate>()
            {
                new GeoCoordinate(12.9316556, 77.6226959),
                new GeoCoordinate(12.8454649, 77.6721007),
                new GeoCoordinate(12.99711, 77.61469),
                new GeoCoordinate(12.965035, 77.5379688),
                new GeoCoordinate(12.97576, 77.62283)
            };
            var cabLocationMap = new Dictionary<string, GeoCoordinate>()
            {
                { "1", coordinates[0] },
                { "2", coordinates[1] },
                { "3", coordinates[2] },
                { "4", coordinates[3] },
                { "5", coordinates[4] }
            };

            var distanceCalculator = new Mock<IDistanceCalculator>();
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[0])).
                Returns(10);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[1])).
                Returns(8);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[2])).
                Returns(2);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[3])).
                Returns(3);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[4])).
                Returns(6);
            var cabLocationRepo = new Mock<ICabLocationRepository>();
            cabLocationRepo.Setup(c => c.GetLocationForCabs(allCabs)).Returns(cabLocationMap);

            var cabLocationService = new CabLocationService(distanceCalculator.Object, cabLocationRepo.Object);
            var cabs = cabLocationService.GetCabsWithinDistance(3, location, allCabs);

            Assert.That(cabs.Count == 2);
            Assert.That(cabs["3"].Equals(coordinates[2]));
            Assert.That(cabs["4"].Equals(coordinates[3]));
        }

        [Test]
        public void ShouldReturnEmptyResultIfCabsDoNotExistWithinSpecifiedDistance()
        {
            var location = new GeoCoordinate(12, 77);
            var allCabs = new List<string>
            {
                "1", "2", "3", "4", "5"
            };
            var coordinates = new List<GeoCoordinate>()
            {
                new GeoCoordinate(12.9316556, 77.6226959),
                new GeoCoordinate(12.8454649, 77.6721007),
                new GeoCoordinate(12.99711, 77.61469),
                new GeoCoordinate(12.965035, 77.5379688),
                new GeoCoordinate(12.97576, 77.62283)
            };
            var cabLocationMap = new Dictionary<string, GeoCoordinate>()
            {
                { "1", coordinates[0] },
                { "2", coordinates[1] },
                { "3", coordinates[2] },
                { "4", coordinates[3] },
                { "5", coordinates[4] }
            };

            var distanceCalculator = new Mock<IDistanceCalculator>();

            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[0])).
                Returns(10);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[1])).
                Returns(8);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[2])).
                Returns(5);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[3])).
                Returns(4);
            distanceCalculator.Setup(d => d.GetDistance(location, coordinates[4])).
                Returns(6);

            var cabLocationRepo = new Mock<ICabLocationRepository>();
            cabLocationRepo.Setup(c => c.GetLocationForCabs(allCabs)).Returns(cabLocationMap);

            var cabLocationService = new CabLocationService(distanceCalculator.Object, cabLocationRepo.Object);
            var cabs = cabLocationService.GetCabsWithinDistance(3, location, allCabs);

            Assert.IsEmpty(cabs);
        }

        [Test]
        public void SetCabLocationShouldUpdateCabLocation()
        {
            var location = new GeoCoordinate(12, 77);

            var distanceCalculator = new Mock<IDistanceCalculator>();
            var cabLocationRepo = new Mock<ICabLocationRepository>();

            var cabLocationService = new CabLocationService(distanceCalculator.Object, cabLocationRepo.Object);
            cabLocationService.SetCabLocation("3", location);

            cabLocationRepo.Verify(c => c.SetCabLocation("3", location));
        }

    }
}
