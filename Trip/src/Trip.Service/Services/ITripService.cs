using System;
using Domain;
using Trip.Service.Models;

namespace Trip.Service.Services
{
    public interface ITripService
    {
        Domain.Trip StartTrip(StartTripRequest startTripRequest);
        void EndTrip(string tripId, GeoCoordinate endLocation, DateTime endTime);
    }
}
