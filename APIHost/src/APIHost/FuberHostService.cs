using System;
using Microsoft.Owin.Hosting;
using Owin;

namespace APIHost
{
    class FuberHostService
    {
        private IDisposable WebAppInstance;

        public void Start()
        {
            var url = "http://*:9000";
            WebAppInstance = WebApp.Start<FuberStartup>(url);
            Console.WriteLine("The application is running on " + url);
        }

        public void Stop()
        {
            if (WebAppInstance != null)
            {
                WebAppInstance.Dispose();
            }
        }
    }

    public class FuberStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
