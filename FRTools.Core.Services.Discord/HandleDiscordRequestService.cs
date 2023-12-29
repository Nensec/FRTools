using System.Net.Http.Headers;
using Azure.Messaging.ServiceBus;
using FRTools.Core.Common.Extentions;
using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Services
{
    public class HandleDiscordRequestService : IHandleDiscordRequestService
    {
        private string _commandsUrl = $"https://discord.com/api/v10/applications/{Environment.GetEnvironmentVariable("DiscordApplicationId")}/commands";

        private readonly ServiceBusSender _serviceBusClient;
        private readonly ILogger<HandleDiscordRequestService> _logger;

        private readonly List<DiscordCommand> _commands = new List<DiscordCommand>();

        public HandleDiscordRequestService(IAzureClientFactory<ServiceBusSender> azureClientFactory, ILogger<HandleDiscordRequestService> logger)
        {
            _serviceBusClient = azureClientFactory.CreateClient(Environment.GetEnvironmentVariable("AzureServiceBusCommandQueue"));
            _logger = logger;
        }

        public AppCommand? ParseJson(string json) => JsonConvert.DeserializeObject<AppCommand>(json);

        public void RegisterCommand(DiscordCommand command)
        {
            _commands.Add(command);
        }

        public async Task RegisterAllCommands()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));
                var response = await client.PutAsJsonAsync(_commandsUrl, _commands.Select(x => x.Command).ToArray());
                _logger.LogInformation("Response: ({0}) {0}", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<DiscordInteractionResponse> ExecuteInteraction(DiscordInteractionRequest interaction)
        {
            var response = await _commands.First(x => x.CommandName == interaction.Data.Name).Execute(interaction);
            if (response is DiscordInteractionResponse.DefferedContentResponse)
                await _serviceBusClient.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(interaction)) { ContentType = "application/json" });
            return response;
        }

        public async Task ExecuteDeferedInteraction(DiscordInteractionRequest interaction) => await _commands.First(x => x.CommandName == interaction.Data.Name).DeferedExecute(interaction);
    }
}