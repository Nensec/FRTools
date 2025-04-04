using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands.Lookup
{
    public class LookupItemCommand : BaseDiscordCommand
    {
        private readonly IFRItemService _itemService;
        private readonly IItemAssetDataService _itemAssetDataService;
        private readonly IFRUserService _userService;
        private readonly ILogger<LookupItemCommand> _logger;

        public LookupItemCommand(IFRItemService itemService, IItemAssetDataService itemAssetDataService, IFRUserService userService, IDiscordService discordService, ILogger<LookupItemCommand> logger) : base(discordService, logger)
        {
            _itemService = itemService;
            _itemAssetDataService = itemAssetDataService;
            _userService = userService;
            _logger = logger;
        }

        public override AppCommand Command => new AppCommand
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
                    Description = "Searches an item in all categories",
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "query",
                            Description = "Do a search with the given search parameter in an item's name or description",
                            Type = AppCommandOptionType.STRING,
                            Required = true,
                        },
                        new AppCommandOption
                        {
                            Name = "dragon_gender",
                            Description = "If the item is equippeable, attempt to show a preview on this gender",
                            Type = AppCommandOptionType.INTEGER,
                            Autocomplete = true,
                            Choices = Enum.GetValues<Gender>().Select(x => new AppCommandOptionChoice{ Name = x.ToString(), Value = (int)x })
                        },
                        DragontypeModernOption(),
                        DragontypeAncientOption()
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
                        },
                        new AppCommandOption
                        {
                            Name = "dragon_gender",
                            Description = "If the item is equippeable, attempt to show a preview on this gender",
                            Type = AppCommandOptionType.INTEGER,
                            Autocomplete = true,
                            Choices = Enum.GetValues<Gender>().Select(x => new AppCommandOptionChoice{ Name = x.ToString(), Value = (int)x })
                        },
                        DragontypeModernOption(),
                        DragontypeAncientOption()
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
                        },
                        new AppCommandOption
                        {
                            Name = "dragon_gender",
                            Description = "If the item is equippeable, attempt to show a preview on this gender",
                            Type = AppCommandOptionType.INTEGER,
                            Autocomplete = true,
                            Choices = Enum.GetValues<Gender>().Select(x => new AppCommandOptionChoice{ Name = x.ToString(), Value = (int)x })
                        },
                        DragontypeModernOption(),
                        DragontypeAncientOption()
                    }
                }
            }.Concat(Enum.GetValues<FRItemCategory>().Where(x => x == FRItemCategory.Equipment || x == FRItemCategory.Trinket).Select(x => new AppCommandOption
            {
                Name = x.ToString().ToLower(),
                Type = AppCommandOptionType.SUB_COMMAND,
                Description = $"Searches an item in the {x} category",
                Options = new[]
                {
                    new AppCommandOption
                    {
                        Name = "query",
                        Description = "Do a search with the given search parameter in an item's name or description",
                        Type = AppCommandOptionType.STRING,
                        Required = true,
                    },
                    new AppCommandOption
                    {
                        Name = "dragon_gender",
                        Description = "If the item is equippeable, attempt to show a preview on this gender",
                        Type = AppCommandOptionType.INTEGER,
                        Autocomplete = true,
                        Choices = Enum.GetValues<Gender>().Select(x => new AppCommandOptionChoice{ Name = x.ToString(), Value = (int)x })
                    },
                    DragontypeModernOption(),
                    DragontypeAncientOption()
                }
            })).Concat(Enum.GetValues<FRItemCategory>().Where(x => x != FRItemCategory.Equipment && x != FRItemCategory.Trinket).Select(x => new AppCommandOption
            {
                Name = x.ToString().ToLower(),
                Type = AppCommandOptionType.SUB_COMMAND,
                Description = $"Searches an item in the {x} category",
                Options = new[]
                {
                    new AppCommandOption
                    {
                        Name = "query",
                        Description = "Do a search with the given search parameter in an item's name or description",
                        Type = AppCommandOptionType.STRING,
                        Required = true,
                    }
                }
            }))
        };

        private static AppCommandOption DragontypeModernOption()
        {
            return new AppCommandOption
            {
                Name = "dragon_type_modern",
                Description = "If the item is equippeable, attempt to show a preview on this modern breed",
                Type = AppCommandOptionType.INTEGER,
                Autocomplete = true,
                Choices = GeneratedFRHelpers.GetModernBreeds().Select(x => new AppCommandOptionChoice { Name = x.ToString(), Value = (int)x })
            };
        }

        private static AppCommandOption DragontypeAncientOption()
        {
            return new AppCommandOption
            {
                Name = "dragon_type_ancient",
                Description = "If the item is equippeable, attempt to show a preview on this ancient breed",
                Type = AppCommandOptionType.INTEGER,
                Autocomplete = true,
                Choices = GeneratedFRHelpers.GetAncientBreeds().Select(x => new AppCommandOptionChoice { Name = x.ToString(), Value = (int)x })
            };
        }

        public override async Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            var command = interaction.Data.Options.First();
            var commandTypeParam = command.Options.First();

            if ((commandTypeParam.Name == "id" || commandTypeParam.Name == "url") && GetIdFromInteraction(interaction) == null)
                await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = "Couldn't find an ID in your request! Either use the name search or provide the correct game-database URL.",
                    Flags = MessageFlags.EPHEMERAL
                });

            await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
            {
                Content = $"Searching for your item in my database, this could take a {(commandTypeParam.Name == "name" ? "while" : "moment")}..",
                Flags = MessageFlags.EPHEMERAL
            });

            Expression<Func<FRItem, bool>> query = x => x.Name.Contains((string)commandTypeParam.Value) || x.Description.Contains((string)commandTypeParam.Value);
            var id = GetIdFromInteraction(interaction);
            switch (commandTypeParam.Name)
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
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"There was no item matching your request",
                        Flags = MessageFlags.EPHEMERAL
                    });
                }

                if ((commandTypeParam.Name == "id" || commandTypeParam.Name == "url") && id.HasValue)
                {
                    var item = await _itemService.FetchItemFromFR((int)id.Value);
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
                await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"I found your item! Please give me a moment while I fetch the data..",
                    Flags = MessageFlags.EPHEMERAL
                });

                var embed = new DiscordEmbed();
                var webhook = new DiscordWebhookFilesRequest
                {
                    PayloadJson = new DiscordWebhookRequest
                    {
                        Content = $"",
                        Embeds = new List<DiscordEmbed>
                        {
                            embed
                        }
                    }
                };


                DragonType? dragonType = null;
                Gender? gender = null;

                if (command.Options.FirstOrDefault(x => x.Name == "dragon_type_modern")?.Value is long dragonTypeModernOption)
                    dragonType = (DragonType)dragonTypeModernOption;
                if (command.Options.FirstOrDefault(x => x.Name == "dragon_type_ancient")?.Value is long dragonTypeAncientOption)
                    dragonType = (DragonType)dragonTypeAncientOption;
                if (command.Options.FirstOrDefault(X => X.Name == "dragon_gender")?.Value is long genderOption)
                    gender = (Gender)genderOption;

                var random = new Random();
                byte[]? itemAsset = await embed.ParseItemForEmbed(random, searchResult[0], _itemAssetDataService, _userService, _logger, dragonType, gender);

                if (itemAsset != null)
                {
                    var fileName = $"asset_{searchResult[0].FRId}.png";

                    webhook.Files.Add(fileName, itemAsset);
                    embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

                }
                var iconAsset = await _itemAssetDataService.GetProxyIcon(searchResult[0].FRId);
                if (iconAsset != null)
                    webhook.Files.Add($"icon_{searchResult[0].FRId}.png", iconAsset);

                var reply = (await DiscordService.ReplyToInteraction(interaction.Token, webhook)).First();
                await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Your lookup result can be found here: https://discord.com/channels/{reply.MessageReference.GuildId}/{reply.ChannelId}/{reply.Id}"
                });
            }
            else
            {
                if (searchResult.Count > 25)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Found {searchResult.Count} items that match `{commandTypeParam.Value}`, but I can only display a preview of 25 items.");
                    var cats = searchResult.GroupBy(x => x.ItemCategory);
                    if (cats.Count() > 1)
                    {
                        foreach (var cat in searchResult.GroupBy(x => x.ItemCategory))
                            sb.AppendLine($"- {cat.Count()} in the category `{cat.Key}`");
                        sb.AppendLine();
                        if (commandTypeParam.Name == "name")
                            sb.AppendLine($"Please refine your search, perhaps use a category filter such as `/lookup {cats.First().Key.ToString().ToLower()} {commandTypeParam.Value}`");
                        else
                            sb.AppendLine($"Please refine your search");
                    }
                    else
                    {
                        sb.AppendLine();
                        sb.AppendLine($"All of the items were part of the category **{cats.First().Key}**");
                        sb.AppendLine("Please refine your search.");
                    }
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"",
                        Embeds = new List<DiscordEmbed>
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
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = $"",
                        Embeds = new List<DiscordEmbed>
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

        private long? GetIdFromInteraction(DiscordInteractionRequest interaction)
        {
            long? id = null;
            var input = interaction.Data.Options.First().Options.First();
            if (input.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
                id = jsonElement.Deserialize<long>();
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