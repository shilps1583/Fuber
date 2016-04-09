using System;
using Topshelf;

namespace APIHost
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                x.Service<FuberHostService>(s =>
                {
                    s.ConstructUsing(name => new FuberHostService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("Fuber Service");
                x.SetDisplayName("Fuber Service");
                x.SetServiceName("Fuber");
                x.StartAutomatically();
            });
        }
    }
}
