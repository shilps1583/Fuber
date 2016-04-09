using System;

namespace Domain
{
    public class Trip
    {
        public string Id { get; private set; }
        public string BookingId { get; private set; }
        public string CabId { get; private set; }
        public GeoCoordinate StartLocation { get; private set; }
        public GeoCoordinate EndLocation { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TripStatus Status { get; private set; }

        public Trip(string id, string bookingId, string cabId, GeoCoordinate startLocation, GeoCoordinate endLocation, DateTime startTime, DateTime endTime, TripStatus status)
        {
            Id = id;
            BookingId = bookingId;
            CabId = cabId;
            StartLocation = startLocation;
            EndLocation = endLocation;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }

        public void SetEndLocation(GeoCoordinate location)
        {
            EndLocation = location;
        }

        public void SetEndTime(DateTime endtime)
        {
            EndTime = endtime;
        }

        public void SetStatus(TripStatus status)
        {
            Status = status;
        }
    }
}
