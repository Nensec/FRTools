using System.Text.Json;
using System.Text.RegularExpressions;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands.Owner
{
    public class ItemBundleConfigCommand : BaseDiscordCommand
    {
        private readonly IFRItemService _fRItemService;

        public ItemBundleConfigCommand(IFRItemService fRItemService, IDiscordInteractionService discordService, ILogger<ItemBundleConfigCommand> logger) : base(discordService, logger)
        {
            _fRItemService = fRItemService;
        }

        public override AppCommand Command => new AppCommand
        {
            Name = "bundle",
            Description = "Configure item bundles",
            DefaultMemberPermissions = Permissions.ADMINISTRATOR,
            DmPermission = false,
            Type = AppCommandType.CHAT_INPUT,
            Options = new[]
            {
                new AppCommandOption
                {
                    Name = "set",
                    Type = AppCommandOptionType.SUB_COMMAND,
                    Description = "Sets an item to be a bundle of other items",
                    Options = new[]
                    {
                        new AppCommandOption
                        {
                            Name = "item",
                            Description = "The item that is a bundle",
                            Type = AppCommandOptionType.INTEGER,
                            Required = true,
                        },
                        new AppCommandOption
                        {
                            Name = "items",
                            Description = "Delimited string of fr items",
                            Type = AppCommandOptionType.STRING,
                            Required = true,
                        }
                    }
                }
            }
        };


        public override Task<DiscordInteractionResponse> Execute(DiscordInteractionRequest interaction)
        {
            if (ulong.TryParse(Environment.GetEnvironmentVariable("DiscordBotOwner"), out var botOwner) && interaction.Member.User.Id == botOwner)
                return base.Execute(interaction);

            return Task.FromResult<DiscordInteractionResponse>(new DiscordInteractionResponse.ContentResponse
            {
                Data = new DiscordInteractionResponseData
                {
                    Content = "Only the bot owner can execute this command.",
                    Flags = MessageFlags.EPHEMERAL
                }
            });
        }

        public override async Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            var parameters = interaction.Data.Options.First().Options;

            var itemsString = (string)parameters["items"].Value;
            var itemMatches = Regex.Matches(itemsString!, @"(?<id>\d+)\D*").Where(x => x.Success).Select(x => x.Groups["id"]).Select(x => int.Parse(x.Value));

            var bundleItemId = long.Parse((string)parameters["item"].Value);

            await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
            {
                Content = $"Fetching the bundle item..",
            });

            var bundleItem = await _fRItemService.GetItem((int)bundleItemId);

            if (bundleItem == null)
            {
                await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = $"Item not found.",
                });
                return;
            }

            await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
            {
                Content = $"Fetching the bundle contents..",
            });

            List<FRItem> fRItems = [];

            foreach (var itemId in itemMatches)
            {
                var item = await _fRItemService.GetItem(itemId) ?? await _fRItemService.FetchItemFromFR(itemId);
                if (item != null)
                    fRItems.Add(item);
            }

            await _fRItemService.SetItemBundle(bundleItem, fRItems);

            await DiscordInteractionService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
            {
                Content = $"Bundle saved.",
            });
        }
    }
}