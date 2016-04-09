using Autofac;
using Booking.Service.Repositories;
using Booking.Service.Services;

namespace Booking.Service
{
    public class BookingAutofacRegistry
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<BookingRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BookingService>().AsImplementedInterfaces().InstancePerRequest();
        }
    }
}
