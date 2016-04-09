using System;
using Domain;
using Newtonsoft.Json;

namespace Booking.Service.Models
{
    public class CreateBookingRequest
    {
        [JsonProperty("UserId")]
        public string UserId { get; set; }
        [JsonProperty("PickupLocation")]
        public GeoCoordinate PickupLocation { get; set; }
        [JsonProperty("Destination")]
        public GeoCoordinate Destination { get; set; }
        [JsonProperty("Time")]
        public DateTime Time { get; set; }
        [JsonProperty("CabType")]
        public string CabType { get; set; }
    }
}
