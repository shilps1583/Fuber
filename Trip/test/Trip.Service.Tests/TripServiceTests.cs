using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Service.Models;
using Booking.Service.Repositories;
using Booking.Service.Services;
using Cab.Services;
using Domain;
using Moq;
using NUnit.Framework;
using Trip.Service.Models;
using Trip.Service.Repositories;
using Trip.Service.Services;

namespace Trip.Service.Tests
{
    [TestFixture]
    public class TripServiceTests
    {
        [Test]
        public void CreateTripShouldUpdateCabLocation()
        {
            var pickupLocation = new GeoCoordinate(12.99711, 77.61469);
            var booking = new Domain.Booking(
                    "1",
                    "1234",
                    "100",
                    pickupLocation,
                    new GeoCoordinate(13, 77),
                    DateTime.Today.AddDays(-1),
                    BookingStatus.Accepted
                    );

            var tripRepo = new Mock<ITripRepository>();
            var cabLocationService = new Mock<ICabLocationService>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            var bookingService = new Mock<IBookingService>();
            bookingService.Setup(b => b.GetBooking("1")).Returns(booking);

            var tripService = new TripService(tripRepo.Object, availableCabsService.Object, bookingService.Object, cabLocationService.Object);
            var startTripRequest = new StartTripRequest()
            {
                BookingId = booking.Id,
                StartLocation = pickupLocation,
                StartTime = DateTime.Now
            };
            
            tripService.StartTrip(startTripRequest);

            tripRepo.Verify(t => t.Save(It.Is<Domain.Trip>(
                trip => trip.BookingId.Equals(booking.Id) && trip.StartLocation.Equals(pickupLocation)
                && trip.CabId.Equals(booking.CabId) && trip.Status == TripStatus.InProgress
                && trip.StartTime.Equals(startTripRequest.StartTime))));
            cabLocationService.Verify(c => c.SetCabLocation(booking.CabId,pickupLocation));
        }

        [Test]
        public void CreateTripShouldThrowIfBookingIsInvalid()
        {
            var pickupLocation = new GeoCoordinate(12.99711, 77.61469);
            var booking = new Domain.Booking(
                    "1",
                    "1234",
                    "100",
                    pickupLocation,
                    new GeoCoordinate(13, 77),
                    DateTime.Today.AddDays(-1),
                    BookingStatus.Cancelled
                    );

            var tripRepo = new Mock<ITripRepository>();
            var cabLocationService = new Mock<ICabLocationService>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            var bookingService = new Mock<IBookingService>();
            bookingService.Setup(b => b.GetBooking("1")).Returns(booking);

            var tripService = new TripService(tripRepo.Object, availableCabsService.Object, bookingService.Object, cabLocationService.Object);
            var startTripRequest = new StartTripRequest()
            {
                BookingId = booking.Id,
                StartLocation = pickupLocation,
                StartTime = DateTime.Now
            };

            Assert.Throws<Exception>(() => tripService.StartTrip(startTripRequest)); 
        }

        [Test]
        public void EndTripShouldUpdateCabLocationAndReturnCabToPool()
        {
            var dropLocation = new GeoCoordinate(13,77);
            var trip = new Domain.Trip("1", "1234", "100", new GeoCoordinate(12, 77), null,
                    DateTime.Now.AddHours(-1), DateTime.MinValue, TripStatus.InProgress);
            var tripRepo = new Mock<ITripRepository>();
            tripRepo.Setup(t => t.Get("1")).Returns(trip);
            var cabLocationService = new Mock<ICabLocationService>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            var bookingService = new Mock<IBookingService>();

            var tripService = new TripService(tripRepo.Object, availableCabsService.Object, bookingService.Object, cabLocationService.Object);
            tripService.EndTrip("1", dropLocation, DateTime.Now);

            tripRepo.Verify(t => t.Save(It.Is<Domain.Trip>(updatedTrip => updatedTrip.Id.Equals(trip.Id))));
            availableCabsService.Verify(a => a.ReturnCabToPool("100"));
            cabLocationService.Verify(c => c.SetCabLocation("100", dropLocation));
        }
    }
}
