using System.Text.RegularExpressions;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class LookupItemCommand : DiscordCommand
    {
        private readonly IFRItemService _fRItemService;
        private readonly IDiscordService _discordService;

        public LookupItemCommand(IFRItemService fRItemService, IDiscordService discordService, ILogger<LookupItemCommand> logger) : base(logger)
        {
            _fRItemService = fRItemService;
            _discordService = discordService;
        }

        public override AppCommand Command { get; } = new AppCommand
        {
            Name = "item",
            Type = AppCommandType.CHAT_INPUT,
            Description = "Looks up a item's information",
            Options = new[]
            {
                new AppCommandOption
                {
                    Name = "name",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Description = "Looks up a item by it's name",
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "name",
                            Description = "The name of the item, this does a search where the given search parameter must exist within the item's name",
                            Type = AppCommandOptionType.STRING,
                            Required = true,
                        }
                    }
                },
                new AppCommandOption
                {
                    Name = "url",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Description = "Looks up a item by it's URL",
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "url",
                            Description = "The (game database) URL of the item",
                            Type = AppCommandOptionType.STRING,
                            Required = true,
                        }
                    }
                },
                new AppCommandOption
                {
                    Name = "id",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Description = "Looks up a item by it's ID",
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "id",
                            Description = "The ID of the item",
                            Type = AppCommandOptionType.INTEGER,
                            Required = true,
                        }
                    }
                }
            }
        };

        public override Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction)
        {
            if (interaction.Data.Options.First().Options.First().Name != "name" && GetIdFromInteraction(interaction) == null)
                return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.ContentResponse
                {
                    Data = new DiscordInteractionResponseData
                    {
                        Content = "Couldn't find an ID in your request! Either use the name search or provide the correct game-database URL.",
                        Flags = MessageFlags.EPHEMERAL
                    }
                });

            return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.DefferedContentResponse());
        }

        public override async Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhook
            {
                Content = $"Searching for your item, this could take a bit..",
                Flags = MessageFlags.EPHEMERAL
            });
        }

        private long? GetIdFromInteraction(DiscordInteractionRequest interaction)
        {
            long? id = 0;
            var input = interaction.Data.Options.First().Options.First();
            if (input.Value is long idParameter)
                id = idParameter;
            else if (input.Name == "url" && input.Value is string url)
            {
                var urlParse = Regex.Match(url, @"/(?<id>\d+)");
                if (urlParse.Success)
                    id = int.Parse(urlParse.Groups["id"].Value);
            }

            return id;
        }
    }
}
