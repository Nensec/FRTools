using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FRTools.Core.Data;
using FRTools.Core.Services;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(FRTools.Core.Functions.Startup))]

namespace FRTools.Core.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_defaultConnection")));
            builder.Services.AddTransient<DataContext>();

            builder.Services.AddSingleton<IAzureStorageService, AzureStorageService>();
            builder.Services.AddSingleton<IAzurePipelineService, AzurePipelineService>();

            builder.Services.AddTransient<IFRUserService, FRUserService>();
            builder.Services.AddTransient<IFRItemService, FRItemService>();

            ConfigureAnnouncers(builder);
        }

        private void ConfigureAnnouncers(IFunctionsHostBuilder builder)
        {
            var announcers = Assembly.GetAssembly(typeof(IAnnouncer)).GetTypes().Where(x => typeof(IAnnouncer).IsAssignableFrom(x) && !x.IsInterface).ToArray();

            foreach(var announcer in announcers)
                builder.Services.AddTransient(announcer);

            builder.Services.AddSingleton<IAnnounceService, AnnounceService>(x =>
            {
                var service = new AnnounceService(x.GetRequiredService<ILogger<AnnounceService>>());
                foreach (var announcer in announcers)                
                    service.RegisterAnnouncer((IAnnouncer)x.GetRequiredService(announcer));                

                return service;
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
