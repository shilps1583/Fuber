using System;
using Autofac;
using Booking.Service;
using Cab;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Serialization.JsonNet;
using Newtonsoft.Json;
using Trip.Service;

namespace APIHost
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);
            StaticConfiguration.DisableErrorTraces = false;

            var builder = new ContainerBuilder();
            CabAutofacRegistry.RegisterDependencies(builder);
            BookingAutofacRegistry.RegisterDependencies(builder);
            TripAutofacRegistry.RegisterDependencies(builder);
            builder.Update(container.ComponentRegistry);
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(c => c.Serializers.Insert(0, typeof(JsonNetSerializer)));
            }
        }

        protected override IRootPathProvider RootPathProvider => new CustomRootPathProvider();
    }

    class CustomRootPathProvider : IRootPathProvider
    {
        private readonly string _rootPath = Environment.CurrentDirectory;

        public string GetRootPath()
        {
            return _rootPath;
        }
    }
}
