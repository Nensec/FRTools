﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FRTools.Core.Data;
using FRTools.Core.Services;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: FunctionsStartup(typeof(FRTools.Core.Functions.Startup))]

namespace FRTools.Core.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
#if DEBUG
                Formatting = Formatting.Indented,
#endif
                NullValueHandling = NullValueHandling.Ignore
            };

            builder.Services.AddDbContext<DataContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_defaultConnection")));
            builder.Services.AddTransient<DataContext>();

            builder.Services.AddSingleton<IAzureStorageService, AzureStorageService>();
            builder.Services.AddSingleton<IAzurePipelineService, AzurePipelineService>();

            builder.Services.AddTransient<IFRUserService, FRUserService>();
            builder.Services.AddTransient<IFRItemService, FRItemService>();

            builder.Services.AddTransient<IItemAssetDataService, ItemAssetDataService>();
            builder.Services.AddTransient<IHtmlService, HtmlService>();
            builder.Services.AddTransient<IConfigService, ConfigService>();

            builder.Services.AddAzureClients(builder =>
            {
                var queueName = Environment.GetEnvironmentVariable("AzureServiceBusCommandQueue");

                builder.AddServiceBusClient(Environment.GetEnvironmentVariable("AZURESBCONNSTR_defaultConnection"));
                builder.AddBlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
                builder.AddClient<ServiceBusSender, ServiceBusClientOptions>((_, provider) => provider.GetRequiredService<ServiceBusClient>().CreateSender(queueName)).WithName(queueName);
                builder.AddClient<BlobServiceClient, BlobClientOptions>((_, provider) => provider.GetRequiredService<BlobServiceClient>()).WithName("frtools");
            });

            builder.Services.AddSingleton<ITumblrService, TumblrService>();

            ConfigureAnnouncers(builder);
            ConfigureDiscord(builder);
        }

        private void ConfigureAnnouncers(IFunctionsHostBuilder builder)
        {
            var announcers = Assembly.GetAssembly(typeof(IAnnouncer)).GetTypes().Where(x => typeof(IAnnouncer).IsAssignableFrom(x) && !x.IsInterface).ToArray();

            foreach (var announcer in announcers)
                builder.Services.AddTransient(announcer);

            builder.Services.AddTransient<AnnounceService>();
            builder.Services.AddSingleton<IAnnounceService>(x =>
            {
                var service = x.GetRequiredService<AnnounceService>();
                foreach (var announcer in announcers)
                    service.RegisterAnnouncer((IAnnouncer)x.GetRequiredService(announcer));

                return service;
            });
        }

        private void ConfigureDiscord(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDiscordService, DiscordService>();

            var discordCommandClasses = Assembly.GetAssembly(typeof(BaseDiscordCommand)).GetTypes().Where(x => typeof(BaseDiscordCommand).IsAssignableFrom(x) && !x.IsAbstract).ToArray();

            foreach (var command in discordCommandClasses)
                builder.Services.AddTransient(command);

            builder.Services.AddTransient<DiscordRequestService>();
            builder.Services.AddSingleton<IDiscordRequestService>(x =>
            {
                var service = x.GetRequiredService<DiscordRequestService>();
                foreach (var command in discordCommandClasses)
                    service.RegisterCommand((BaseDiscordCommand)x.GetRequiredService(command));

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
