using Autofac;
using Booking.Service.Services;
using Trip.Service.Repositories;
using Trip.Service.Services;

namespace Trip.Service
{
    public class TripAutofacRegistry
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<TripRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TripService>().AsImplementedInterfaces().InstancePerRequest();
        }
    }
}
