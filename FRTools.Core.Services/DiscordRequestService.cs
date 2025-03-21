﻿using System.Net.Http.Headers;
using Azure.Messaging.ServiceBus;
using FRTools.Core.Common.Extentions;
using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Services
{
    public class DiscordRequestService : IDiscordRequestService
    {
        private readonly string _commandsUrl = $"https://discord.com/api/v10/applications/{Environment.GetEnvironmentVariable("DiscordApplicationId")}/commands";

        private readonly ServiceBusSender _serviceBusClient;
        private readonly IDiscordService _discordService;
        private readonly ILogger<DiscordRequestService> _logger;

        private readonly List<BaseDiscordCommand> _commands = [];

        public DiscordRequestService(IAzureClientFactory<ServiceBusSender> azureClientFactory, IDiscordService discordService, ILogger<DiscordRequestService> logger)
        {
            _serviceBusClient = azureClientFactory.CreateClient(Environment.GetEnvironmentVariable("AzureServiceBusCommandQueue"));
            _discordService = discordService;
            _logger = logger;
        }

        public void RegisterCommand(BaseDiscordCommand command)
        {
            _logger.LogDebug("Registering command: '{0}' {1}", command.CommandName, command.ComponentInteractionCommandNames != null ? string.Join(", ", command.ComponentInteractionCommandNames) : "");
            _commands.Add(command);
        }

        public async Task RegisterAllCommands()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));
                var response = await client.PutAsJsonAsync(_commandsUrl, _commands.Select(x => x.Command).ToArray());
                _logger.LogInformation("Response: ({0}) {1}", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<DiscordInteractionResponse> ExecuteInteraction(DiscordInteractionRequest interaction)
        {
            _logger.LogDebug("Attempting to execute command:\n\tName: '{0}'\n\tCustomId: '{1}'", interaction.Data.Name, interaction.Data.CustomId);
            try
            {
                var response = await _commands.First(x => x.CommandName == interaction.Data.Name || x.ComponentInteractionCommandNames?.Contains(interaction.Data.CustomId) == true).Execute(interaction);
                if (response is DiscordInteractionResponse.DefferedContentResponse)
                    await _serviceBusClient.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(interaction)) { ContentType = "application/json" });

                return response;
            }
            catch
            {
                return new DiscordInteractionResponse.ContentResponse
                {
                    Data = new DiscordInteractionResponseData
                    {
                        Content = ":octagonal_sign: Something went wrong executing this command, don't worry though <@107155889563115520> is on it!.. probably?"
                    }
                };
            }
        }

        public async Task ExecuteDeferedInteraction(DiscordInteractionRequest interaction)
        {
            try
            {
                await _commands.First(x => x.CommandName == interaction.Data.Name).DeferedExecute(interaction);
            }
            catch
            {
                await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = ":octagonal_sign: Something went wrong executing this command, don't worry though <@107155889563115520> is on it!.. probably?"
                });
            }
        }
    }
}