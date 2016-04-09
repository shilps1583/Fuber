using System;
using Domain;

namespace Trip.Service.Models
{
    public class StartTripRequest
    {
        public string BookingId { get; set; }
        public GeoCoordinate StartLocation { get; set; }
        public DateTime StartTime { get; set; }
    }
}
