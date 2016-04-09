using Autofac;
using Cab.Repositories;
using Cab.Services;

namespace Cab
{
    public class CabAutofacRegistry
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<DistanceCalculator>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CabLocationRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AvailableCabsRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CabLocationService>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<AvailableCabsService>().AsImplementedInterfaces().InstancePerRequest();
        }
    }
}
