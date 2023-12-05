using System;
using System.IO;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Services;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FRTools.Core.Functions.Startup))]

namespace FRTools.Core.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_defaultConnection")));
            builder.Services.AddTransient<DataContext>();

            builder.Services.AddTransient<IAzureStorageService, AzureStorageService>();
            builder.Services.AddTransient<IFRUserService, FRUserService>();
            builder.Services.AddTransient<IFRItemService, FRItemService>();

            builder.Services.AddOptions();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var config = builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            //var tumblrConfig = new TumblrConfig();
            //config.Bind(nameof(TumblrConfig), tumblrConfig);
        }
    }
}
