using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Cab.API.Models
{
    public class CabsResponse
    {
        public string Id { get; set; }
        public string VehicleType { get; set; }
        public string Type { get; set; }
        public GeoCoordinate CurrentLocation { get; set; }
    }
}
