using System;

namespace Domain
{
    public class Booking
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string CabId { get; private set; }
        public GeoCoordinate PickupLocation { get; private set; }
        public GeoCoordinate Destination { get; private set; }
        public DateTime Time { get; private set; }
        public BookingStatus Status { get; private set; }

        public Booking(string id, string userId, string cabId, GeoCoordinate pickupLocation, GeoCoordinate destination, DateTime time, BookingStatus status)
        {
            Id = id;
            UserId = userId;
            CabId = cabId;
            PickupLocation = pickupLocation;
            Destination = destination;
            Time = time;
            Status = status;
        }
    }
}
