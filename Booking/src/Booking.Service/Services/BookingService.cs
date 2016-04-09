using System;
using System.Collections.Generic;
using Booking.Service.Models;
using Booking.Service.Repositories;
using Cab.Services;
using Domain;

namespace Booking.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository BookingRepository;
        private readonly IAvailableCabsService AvailableCabsService;

        public BookingService(IBookingRepository bookingRepository, IAvailableCabsService availableCabsService)
        {
            BookingRepository = bookingRepository;
            AvailableCabsService = availableCabsService;
        }

        public Domain.Booking CreateBooking(CreateBookingRequest createBookingRequest)
        {
            var cabTypes = new List<CabType>();
            if (!string.IsNullOrEmpty(createBookingRequest.CabType))
            {
                CabType cabType;
                Enum.TryParse(createBookingRequest.CabType, true, out cabType);
                cabTypes.Add(cabType);
            }
            else
            {
                cabTypes.Add(CabType.Regular);
                cabTypes.Add(CabType.Pink);
            }
            var availableCab = AvailableCabsService.GetNearestCab(createBookingRequest.PickupLocation, cabTypes.ToArray());
            if (availableCab != null)
            {
                try
                {
                    var booking = new Domain.Booking(
                    Guid.NewGuid().ToString(),
                    createBookingRequest.UserId,
                    availableCab.Id,
                    createBookingRequest.PickupLocation,
                    createBookingRequest.Destination,
                    createBookingRequest.Time,
                    BookingStatus.Accepted
                    );
                    BookingRepository.Save(booking);
                    return booking;
                }
                catch (Exception)
                {
                    AvailableCabsService.ReturnCabToPool(availableCab);
                    throw;
                }
            }
            throw new Exception("No cars available");
        }
        
        public Domain.Booking GetBooking(string bookingId)
        {
            return BookingRepository.Get(bookingId);
        }

        public IEnumerable<Domain.Booking> GetAllBookings()
        {
            return BookingRepository.GetAll();
        }
    }
}
