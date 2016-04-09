using System.Collections.Generic;
using System.Linq;

namespace Booking.Service.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        static readonly List<Domain.Booking> allBookings = new List<Domain.Booking>();
        public Domain.Booking Save(Domain.Booking booking)
        {
            allBookings.Add(booking);
            return booking;
        }

        public Domain.Booking Get(string bookingId)
        {
            return allBookings.FirstOrDefault(b => b.Id.Equals(bookingId));
        }

        public IEnumerable<Domain.Booking> GetAll()
        {
            return allBookings;
        }
    }
}
