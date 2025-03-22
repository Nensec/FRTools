using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FRTools.Core.Data;
using FRTools.Core.Services;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace FRTools.Core.Functions
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
#if DEBUG
                Formatting = Formatting.Indented,
#endif
                NullValueHandling = NullValueHandling.Ignore
            };

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();

                    services.AddDbContext<DataContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_defaultConnection")), contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Singleton);
                    services.AddTransient<DataContext>();

                    services.AddSingleton<IAzureStorageService, AzureStorageService>();
                    services.AddSingleton<IAzurePipelineService, AzurePipelineService>();

                    services.AddTransient<IFRUserService, FRUserService>();
                    services.AddTransient<IFRItemService, FRItemService>();

                    services.AddTransient<IItemAssetDataService, ItemAssetDataService>();
                    services.AddTransient<IHtmlService, HtmlService>();
                    services.AddTransient<IConfigService, ConfigService>();

                    services.AddAzureClients(builder =>
                    {
                        var queueName = Environment.GetEnvironmentVariable("AzureServiceBusCommandQueue");

                        builder.AddServiceBusClient(Environment.GetEnvironmentVariable("AZURESBCONNSTR_defaultConnection"));
                        builder.AddBlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
                        builder.AddClient<ServiceBusSender, ServiceBusClientOptions>((_, provider) => provider.GetRequiredService<ServiceBusClient>().CreateSender(queueName)).WithName(queueName);
                        builder.AddClient<BlobServiceClient, BlobClientOptions>((_, provider) => provider.GetRequiredService<BlobServiceClient>()).WithName("frtools");
                    });

                    services.AddSingleton<ITumblrService, TumblrService>();

                    ConfigureAnnouncers(services);
                    ConfigureDiscord(services);
                })
                .Build();

            await host.RunAsync();
        }

        private static void ConfigureAnnouncers(IServiceCollection services)
        {
            var announcers = Assembly.GetAssembly(typeof(IAnnouncer))!.GetTypes().Where(x => typeof(IAnnouncer).IsAssignableFrom(x) && !x.IsInterface).ToArray();

            foreach (var announcer in announcers)
                services.AddTransient(announcer);

            services.AddTransient<AnnounceService>();
            services.AddSingleton<IAnnounceService>(x =>
            {
                var service = x.GetRequiredService<AnnounceService>();
                foreach (var announcer in announcers)
                    service.RegisterAnnouncer((IAnnouncer)x.GetRequiredService(announcer));

                return service;
            });
        }

        private static void ConfigureDiscord(IServiceCollection services)
        {
            services.AddSingleton<IDiscordService, DiscordService>();

            var discordCommandClasses = Assembly.GetAssembly(typeof(BaseDiscordCommand))!.GetTypes().Where(x => typeof(BaseDiscordCommand).IsAssignableFrom(x) && !x.IsAbstract).ToArray();

            foreach (var command in discordCommandClasses)
                services.AddTransient(command);

            services.AddTransient<DiscordRequestService>();
            services.AddSingleton<IDiscordRequestService>(x =>
            {
                var service = x.GetRequiredService<DiscordRequestService>();
                foreach (var command in discordCommandClasses)
                    service.RegisterCommand((BaseDiscordCommand)x.GetRequiredService(command));

                return service;
            });
        }
    }
}