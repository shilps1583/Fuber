using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using NUnit.Framework;

namespace Cab.Tests
{
    [TestFixture]
    public class DistanceCalculatorTests
    {
        [Test]
        public void ShouldCalculateDistanceOnPythagoreanTheorem()
        {
            var point1 = new GeoCoordinate(-2,1);
            var point2 = new GeoCoordinate(1,5);
            var distanceCalculator = new DistanceCalculator();
            var distance = distanceCalculator.GetDistance(point1, point2);
            Assert.That(distance == 5.0);
        }
    }
}
