using System;
using Booking.Service.Services;
using Cab;
using Cab.Services;
using Domain;
using Trip.Service.Models;
using Trip.Service.Repositories;

namespace Trip.Service.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository TripRepository;
        private readonly IAvailableCabsService AvailableCabsService;
        private readonly IBookingService BookingService;
        private readonly ICabLocationService CabLocationService;

        public TripService(ITripRepository tripRepository, 
            IAvailableCabsService availableCabsService,
            IBookingService bookingService,
            ICabLocationService cabLocationService)
        {
            TripRepository = tripRepository;
            AvailableCabsService = availableCabsService;
            BookingService = bookingService;
            CabLocationService = cabLocationService;
        }

        public Domain.Trip StartTrip(StartTripRequest startTripRequest)
        {
            var booking = BookingService.GetBooking(startTripRequest.BookingId);
            if (booking != null && booking.Status == BookingStatus.Accepted)
            {
                var trip = new Domain.Trip(Guid.NewGuid().ToString(), booking.Id, booking.CabId, startTripRequest.StartLocation, null,
                    startTripRequest.StartTime, DateTime.MinValue, TripStatus.InProgress);
                TripRepository.Save(trip);
                CabLocationService.SetCabLocation(booking.CabId,trip.StartLocation);
                return trip;
            }
            throw new Exception("Invalid booking id");
        }

        public void EndTrip(string tripId, GeoCoordinate endLocation, DateTime endTime)
        {
            var trip = TripRepository.Get(tripId);
            if (trip != null)
            {
                trip.SetEndLocation(endLocation);
                trip.SetEndTime(endTime);
                trip.SetStatus(TripStatus.Completed);
                TripRepository.Save(trip);
                AvailableCabsService.ReturnCabToPool(trip.CabId);
                CabLocationService.SetCabLocation(trip.CabId, endLocation);
            }
        }
    }
}
