using System;
using Booking.Service.Models;
using Booking.Service.Services;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json;

namespace Booking.API.Modules
{
    public class BookingModule : NancyModule
    {
        private readonly IBookingService BookingService;

        public BookingModule(IBookingService bookingService) : base("/bookings")
        {
            BookingService = bookingService;

            Post["/"] = (context) =>
            {
                return CreateBooking();
            };

            Get["/"] = (context) =>
            {
                return GetBookings();
            };
        }

        private dynamic CreateBooking()
        {
            var responseNegotiator = Negotiate.WithHeader("Content-Type", "application/json");
            try
            {
                var createBookingRequest = JsonConvert.DeserializeObject<CreateBookingRequest>(Request.Body.AsString());
                var booking = BookingService.CreateBooking(createBookingRequest);
                responseNegotiator.WithModel(booking).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                responseNegotiator.WithModel(ex.Message).WithStatusCode(HttpStatusCode.InternalServerError);
            }
            return responseNegotiator;
        }

        private dynamic GetBookings()
        {
            var responseNegotiator = Negotiate.WithHeader("Content-Type", "application/json");
            try
            {
                var bookings = BookingService.GetAllBookings();
                responseNegotiator.WithModel(bookings).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                responseNegotiator.WithModel(ex.Message).WithStatusCode(HttpStatusCode.InternalServerError);
            }
            return responseNegotiator;
        }
    }
}
