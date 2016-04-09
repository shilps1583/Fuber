using System;
using System.Collections.Generic;
using Booking.Service.Models;

namespace Booking.Service.Services
{
    public interface IBookingService
    {
        Domain.Booking CreateBooking(CreateBookingRequest createBookingRequest);
        Domain.Booking GetBooking(string bookingId);
        IEnumerable<Domain.Booking> GetAllBookings();
    }
}
