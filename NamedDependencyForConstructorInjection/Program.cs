using DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamedDependencyForConstructorInjection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IAppleItem>((_) =>
                    {
                        var apple = new WorkerItem { Name = "Apple" };
                        return Proxy.CreateInstance<IAppleItem, WorkerItem>(apple);
                    });
                    services.AddScoped<IPearItem>((_) =>
                    {
                        var pear = new WorkerItem { Name = "Pear" };
                        return Proxy.CreateInstance<IPearItem, WorkerItem>(pear);
                    });
                    services.AddScoped<WorkerService>();
                    services.AddHostedService<Worker>();
                });
    }
}
