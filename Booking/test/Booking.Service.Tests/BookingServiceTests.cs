using System;
using System.Collections.Generic;
using Booking.Service.Models;
using Booking.Service.Repositories;
using Booking.Service.Services;
using Cab.Services;
using Domain;
using Moq;
using NUnit.Framework;

namespace Booking.Service.Tests
{
    [TestFixture]
    public class BookingServiceTests
    {
        [Test]
        public void ShouldCreateBookingIfCabsAreAvailable()
        {
            var pickupLocation = new GeoCoordinate(12.99711, 77.61469);
            var bookingRepo = new Mock<IBookingRepository>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            var availableCab = new Domain.Cab("3", "Toyota Etios", CabType.Pink);
            availableCabsService.Setup(a => a.GetNearestCab(pickupLocation, new CabType[] {CabType.Regular, CabType.Pink})).Returns(availableCab);

            var bookingService = new BookingService(bookingRepo.Object, availableCabsService.Object);
            var createBookingRequest = new CreateBookingRequest()
            {
                PickupLocation = pickupLocation,
                Destination = new GeoCoordinate(13, 77),
                Time = DateTime.Now,
                UserId = "1"
            };
            var booking = bookingService.CreateBooking(createBookingRequest);

            Assert.That(booking != null);
            Assert.That(booking.CabId == availableCab.Id);
            Assert.That(booking.UserId == createBookingRequest.UserId);
            Assert.That(booking.PickupLocation.Equals(createBookingRequest.PickupLocation));
            Assert.That(booking.Destination.Equals(createBookingRequest.Destination));
            Assert.That(booking.Time.Equals(createBookingRequest.Time));

            availableCabsService.Verify(a => a.GetNearestCab(createBookingRequest.PickupLocation, new CabType[] { CabType.Regular, CabType.Pink }));
            bookingRepo.Verify(b => b.Save(booking));
        }

        [Test]
        public void ShouldCreateBookingByCabType()
        {
            var pickupLocation = new GeoCoordinate(12.99711, 77.61469);
            var bookingRepo = new Mock<IBookingRepository>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            var availableCab = new Domain.Cab("3", "Toyota Etios", CabType.Pink);
            availableCabsService.Setup(a => a.GetNearestCab(pickupLocation, new[] { CabType.Pink })).Returns(availableCab);

            var bookingService = new BookingService(bookingRepo.Object, availableCabsService.Object);
            var createBookingRequest = new CreateBookingRequest()
            {
                PickupLocation = pickupLocation,
                Destination = new GeoCoordinate(13, 77),
                Time = DateTime.Now,
                UserId = "1",
                CabType = "Pink"
            };
            var booking = bookingService.CreateBooking(createBookingRequest);

            Assert.That(booking != null);
            Assert.That(booking.CabId == availableCab.Id);
            Assert.That(booking.UserId == createBookingRequest.UserId);
            Assert.That(booking.PickupLocation.Equals(createBookingRequest.PickupLocation));
            Assert.That(booking.Destination.Equals(createBookingRequest.Destination));
            Assert.That(booking.Time.Equals(createBookingRequest.Time));

            availableCabsService.Verify(a => a.GetNearestCab(createBookingRequest.PickupLocation, new[] { CabType.Pink }));
            bookingRepo.Verify(b => b.Save(booking));
        }

        [Test]
        public void ShouldThrowExceptionIfCabsAreNotAvailable()
        {
            var pickupLocation = new GeoCoordinate(12.99711, 77.61469);
            var bookingRepo = new Mock<IBookingRepository>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            availableCabsService.Setup(a => a.GetNearestCab(pickupLocation, new CabType[] { CabType.Regular, CabType.Pink })).Returns(() => { return null; });

            var bookingService = new BookingService(bookingRepo.Object, availableCabsService.Object);
            var createBookingRequest = new CreateBookingRequest()
            {
                PickupLocation = pickupLocation,
                Destination = new GeoCoordinate(13, 77),
                Time = DateTime.Now,
                UserId = "1"
            };
            Assert.Throws<Exception>(() => bookingService.CreateBooking(createBookingRequest));

            availableCabsService.Verify(a => a.GetNearestCab(createBookingRequest.PickupLocation, new CabType[] { CabType.Regular, CabType.Pink }));
        }

        [Test]
        public void ShouldReturnCabToPoolIfCreateBookingFails()
        {
            var pickupLocation = new GeoCoordinate(12.99711, 77.61469);
            var bookingRepo = new Mock<IBookingRepository>();
            bookingRepo.Setup(b => b.Save(It.IsAny<Domain.Booking>())).Throws<Exception>();
            var availableCabsService = new Mock<IAvailableCabsService>();
            var availableCab = new Domain.Cab("3", "Toyota Etios", CabType.Pink);
            availableCabsService.Setup(a => a.GetNearestCab(pickupLocation, new CabType[] { CabType.Regular, CabType.Pink })).Returns(availableCab);

            var bookingService = new BookingService(bookingRepo.Object, availableCabsService.Object);
            var createBookingRequest = new CreateBookingRequest()
            {
                PickupLocation = pickupLocation,
                Destination = new GeoCoordinate(13, 77),
                Time = DateTime.Now,
                UserId = "1"
            };
            Assert.Throws<Exception>(() => bookingService.CreateBooking(createBookingRequest));

            availableCabsService.Verify(a => a.GetNearestCab(createBookingRequest.PickupLocation, new CabType[] { CabType.Regular, CabType.Pink }));
            availableCabsService.Verify(a => a.ReturnCabToPool(availableCab));
        }

        [Test]
        public void ShouldGetBookingById()
        {
            var expectedBooking = new Domain.Booking(
                    "100",
                    "1234",
                    "101",
                    new GeoCoordinate(13,77),
                    new GeoCoordinate(14,77),
                    DateTime.Now,
                    BookingStatus.Accepted
                    );
            var bookingRepo = new Mock<IBookingRepository>();
            bookingRepo.Setup(b => b.Get("100")).Returns(expectedBooking);
            var availableCabsService = new Mock<IAvailableCabsService>();

            var bookingService = new BookingService(bookingRepo.Object, availableCabsService.Object);
            var actual = bookingService.GetBooking("100");
            bookingRepo.Verify(b => b.Get("100"));
        }

        [Test]
        public void ShouldGetAllBookings()
        {
            var bookingList = new List<Domain.Booking>
            {
                new Domain.Booking(
                    "100",
                    "1234",
                    "101",
                    new GeoCoordinate(13, 77),
                    new GeoCoordinate(14, 77),
                    DateTime.Now,
                    BookingStatus.Accepted
                    ),
                new Domain.Booking(
                    "102",
                    "1235",
                    "103",
                    new GeoCoordinate(13, 77),
                    new GeoCoordinate(14, 77),
                    DateTime.Now,
                    BookingStatus.Accepted),
                new Domain.Booking(
                    "104",
                    "1236",
                    "105",
                    new GeoCoordinate(13, 77),
                    new GeoCoordinate(14, 77),
                    DateTime.Now,
                    BookingStatus.Accepted
                    )
            };
            var bookingRepo = new Mock<IBookingRepository>();
            bookingRepo.Setup(b => b.GetAll()).Returns(bookingList);
            var availableCabsService = new Mock<IAvailableCabsService>();

            var bookingService = new BookingService(bookingRepo.Object, availableCabsService.Object);
            var actual = bookingService.GetAllBookings();
            CollectionAssert.AreEqual(bookingList, actual);
        }
    }
}
