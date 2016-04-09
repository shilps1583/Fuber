using System;
using System.Collections.Generic;
using System.Linq;

namespace Trip.Service.Repositories
{
    public class TripRepository : ITripRepository
    {
        static readonly List<Domain.Trip> allTrips = new List<Domain.Trip>();

        public Domain.Trip Save(Domain.Trip trip)
        {
            allTrips.Add(trip);
            return trip;
        }

        public Domain.Trip Get(string tripId)
        {
            return allTrips.FirstOrDefault(t => t.Id.Equals(tripId));
        }
    }
}
