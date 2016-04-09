using System.Collections.Generic;

namespace Booking.Service.Repositories
{
    public interface IBookingRepository
    {
        Domain.Booking Save(Domain.Booking booking);
        Domain.Booking Get(string bookingId);
        IEnumerable<Domain.Booking> GetAll();
    }
}