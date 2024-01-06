using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using FRTools.Core.Common;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
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
        private readonly IFRItemService _itemService;
        private readonly IFRUserService _userService;
        private readonly IDiscordService _discordService;
        private readonly ILogger<LookupItemCommand> _logger;

        public LookupItemCommand(IFRItemService itemService, IFRUserService userService, IDiscordService discordService, ILogger<LookupItemCommand> logger) : base(logger)
        {
            _itemService = itemService;
            _userService = userService;
            _discordService = discordService;
            _logger = logger;
        }

        public override AppCommand Command { get; } = new AppCommand
        {
            Name = "lookup",
            Type = AppCommandType.CHAT_INPUT,
            Description = "Looks up a item's information",
            Options = new[]
            {
                new AppCommandOption
                {
                    Name = "item",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Description = "Looks up a item by it's name",
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "name",
                            Description = "The name of the item, this does a search where the given search parameter must exist within the item's name or description",
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
            }.Concat(Enum.GetValues<FRItemCategory>().Select(x => new AppCommandOption
            {
                Name = x.ToString().ToLower(),
                Type = AppCommandOptionType.SUB_COMMAND,
                Description = $"Looks up a item by it's name in the {x} category",
                Options = new[]
                {
                    new AppCommandOption
                    {
                        Name = "name",
                        Description = "The name of the item, this does a search where the given search parameter must exist within the item's name or description",
                        Type = AppCommandOptionType.STRING,
                        Required = true,
                    }
                }
            }))
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
            var command = interaction.Data.Options.First().Options.First();

            await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhook
            {
                Content = $"Searching for your item, this could take a {(command.Name == "name" ? "while" : "moment")}..",
                Flags = MessageFlags.EPHEMERAL
            });

            Expression<Func<FRItem, bool>> query = x => x.Name.Contains((string)command.Value) || x.Description.Contains((string)command.Value);
            var id = GetIdFromInteraction(interaction);
            switch (command.Name)
            {
                case "url":
                case "id":
                    query = x => x.FRId == id;
                    break;

            }

            var searchResult = (await _itemService.SearchItems(query)).ToList();

            if (searchResult.Count == 0)
            {
                async Task NoItemFound()
                {
                    await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhook
                    {
                        Content = $"There was no item matching your request",
                        Flags = MessageFlags.EPHEMERAL
                    });
                }

                if ((command.Name == "id" || command.Name == "url") && id.HasValue)
                {
                    var item = await _itemService.FetchItemFromFR(id.Value);
                    if (item != null)
                        searchResult = new() { item };
                    else
                    {
                        await NoItemFound();
                        return;
                    }
                }
                else
                {
                    await NoItemFound();
                    return;
                }
            }

            if (searchResult.Count == 1)
            {
                await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhook
                {
                    Content = $"I found your item! Please give me a moment while I fetch the data..",
                    Flags = MessageFlags.EPHEMERAL
                });

                var embed = new DiscordEmbed();
                var webhook = new DiscordWebhookFiles
                {
                    PayloadJson = new DiscordWebhook
                    {
                        Content = $"",
                        Embeds = new[]
                        {
                            embed
                        }
                    }
                };

                var random = new Random();
                byte[]? itemAsset = await embed.ParseItemForEmbed(random, searchResult[0], _userService, _logger);

                if (itemAsset != null)
                {
                    var fileName = $"asset_{searchResult[0].FRId}.png";

                    webhook.Files.Add(fileName, itemAsset);
                    embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

                }
                using (var client = new HttpClient())
                {
                    var iconAsset = await client.GetByteArrayAsync(Helpers.GetProxyIconUrl(searchResult[0].FRId));
                    webhook.Files.Add($"icon_{searchResult[0].FRId}.png", iconAsset);
                }
                await _discordService.ReplyToInteraction(interaction.Token, webhook);
            }
            else
            {
                if (searchResult.Count > 25)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Found {searchResult.Count} items that match `{command.Value}`, but I can only display a preview of 25 items.");
                    var cats = searchResult.GroupBy(x => x.ItemCategory);
                    if (cats.Count() > 1)
                    {
                        foreach (var cat in searchResult.GroupBy(x => x.ItemCategory))
                            sb.AppendLine($"- {cat.Count()} in the category `{cat.Key}`");
                        sb.AppendLine();
                        if (command.Name == "name")
                            sb.AppendLine($"Please refine your search, perhaps use a category filter such as `/lookup {cats.First().Key.ToString().ToLower()} {command.Value}`");
                        else
                            sb.AppendLine($"Please refine your search");
                    }
                    else
                    {
                        sb.AppendLine();
                        sb.AppendLine($"All of the items were part of the category **{cats.First().Key}**");
                        sb.AppendLine("Please refine your search.");
                    }
                    await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhook
                    {
                        Content = $"",
                        Embeds = new[]
                        {
                            new DiscordEmbed
                            {
                                Title = "Too many results matching your query",
                                Description = sb.ToString()
                            }
                        }
                    });
                }
                else
                {
                    await _discordService.EditInitialInteraction(interaction.Token, new DiscordWebhook
                    {
                        Content = $"",
                        Embeds = new[]
                        {
                            new DiscordEmbed
                            {
                                Description = $"Found {searchResult.Count} items that match your query. Please look at the items below and use `/lookup id <id>` to view it's details.",
                                Fields = searchResult.Select(x => new DiscordEmbedField { Value = $"FR Id: [{x.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, x.FRId)}) ({x.ItemCategory.ToString().ToLower()})", Name = x.Name, Inline = true }).ToList()
                            }
                        }
                    });
                }
            }
        }

        private int? GetIdFromInteraction(DiscordInteractionRequest interaction)
        {
            int? id = 0;
            var input = interaction.Data.Options.First().Options.First();
            if (input.Value is int idParameter)
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