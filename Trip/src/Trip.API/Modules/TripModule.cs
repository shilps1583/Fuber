using System;
using Domain;
using Trip.Service.Models;
using Trip.Service.Services;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace Trip.API.Modules
{
    public class TripModule : NancyModule
    {
        private readonly ITripService TripService;

        public TripModule(ITripService tripService) : base("/trips")
        {
            TripService = tripService;

            Post["/"] = _ =>
            {
                return CreateTrip();
            };

            Post["/{tripId}"] = (parameters) =>
            {
                return EndTrip(parameters.tripId);
            };
        }

        private dynamic CreateTrip()
        {
            var responseNegotiator = Negotiate.WithHeader("Content-Type", "application/json");
            try
            {
                var startTripRequest = JsonConvert.DeserializeObject<StartTripRequest>(Request.Body.AsString());
                var trip = TripService.StartTrip(startTripRequest);
                responseNegotiator.WithModel(trip).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                responseNegotiator.WithModel(ex.Message).WithStatusCode(HttpStatusCode.InternalServerError);
            }
            return responseNegotiator;
        }

        private dynamic EndTrip(string tripId)
        {
            var responseNegotiator = Negotiate.WithHeader("Content-Type", "application/json");
            try
            {
                var endTripRequest = JsonConvert.DeserializeObject<dynamic>(Request.Body.AsString());
                var endLocation = new GeoCoordinate((double)endTripRequest.EndLocation.Latitude, (double)endTripRequest.EndLocation.Longitude);
                var endTime = DateTime.Parse(endTripRequest.EndTime.ToString());
                TripService.EndTrip(tripId, endLocation, endTime);
                responseNegotiator.WithModel("Trip ended succesfully").WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                responseNegotiator.WithModel(ex.Message).WithStatusCode(HttpStatusCode.InternalServerError);
            }
            return responseNegotiator;
        }
    }
}
