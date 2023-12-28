using System.Text.RegularExpressions;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.RequestModels;
using FRTools.Core.Services.Discord.DiscordModels.ResponseModels;
using FRTools.Core.Services.Discord.Interfaces;
using FRTools.Core.Services.DiscordModels;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands
{
    public class DragonCommand : DiscordCommand
    {
        private readonly IDiscordService _discordService;

        public DragonCommand(ILogger<DragonCommand> logger, IDiscordService discordService) : base(logger)
        {
            _discordService = discordService;
        }

        public override AppCommand CreateCommand()
        {
            return new AppCommand
            {
                Name = "dragon",
                Type = AppCommandType.CHAT_INPUT,
                Description = "Looks up a dragon's information",
                Options = new[]
                {
                    new AppCommandOption
                    {
                        Name = "url",
                        Type = AppCommandOptionType.SUB_COMMAND,
                        Description = "Looks up a dragon by it's URL",
                        Options = new[]
                        {
                            new AppCommandOption
                            {
                                Name = "url",
                                Description = "The URL of the dragon",
                                Type = AppCommandOptionType.STRING,
                                Required = true,
                            }
                        }
                    },
                    new AppCommandOption
                    {
                        Name = "id",
                        Type = AppCommandOptionType.SUB_COMMAND,
                        Description = "Looks up a dragon by it's ID",
                        Options = new[]
                        {
                            new AppCommandOption
                            {
                                Name = "id",
                                Description = "The URL of the dragon",
                                Type = AppCommandOptionType.INTEGER,
                                Required = true,
                            }
                        }
                    }
                }
            };
        }

        public override Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction)
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
            if (id == null)
                return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.ContentResponse
                {
                    Data = new DiscordInteractionResponseData
                    {
                        Content = "Couldn't find an ID in your request!", Flags = MessageFlags.EPHEMERAL
                    }
                });

            return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.DefferedContentResponse());
        }

        public override Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            return base.DeferedExecute(interaction);
        }
    }
}