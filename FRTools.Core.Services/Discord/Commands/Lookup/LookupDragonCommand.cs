using System.Text.Json;
using System.Text.RegularExpressions;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Services.Discord.DiscordModels.CommandModels;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Discord.Commands.Lookup
{
    public class LookupDragonCommand : BaseDiscordCommand
    {
        private readonly IItemAssetDataService _itemAssetDataService;

        public LookupDragonCommand(IItemAssetDataService itemAssetDataService, IDiscordService discordService, ILogger<LookupDragonCommand> logger) : base(discordService, logger)
        {
            _itemAssetDataService = itemAssetDataService;
        }

        public override AppCommand Command => new AppCommand
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
                            Description = "The ID of the dragon",
                            Type = AppCommandOptionType.INTEGER,
                            Required = true,
                        }
                    }
                }
            }
        };

        public override async Task DeferedExecute(DiscordInteractionRequest interaction)
        {
            long? id = GetIdFromInteraction(interaction);
            if (id == null)
            {
                await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = "Given ID is invalid",
                    Flags = MessageFlags.EPHEMERAL
                });

                return;
            }

            await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
            {
                Content = $"Looking up dragon {id}, gimme a moment..",
                Flags = MessageFlags.EPHEMERAL
            });

            var client = new HtmlWeb();
            try
            {
                var dragonProfileDoc = client.Load(string.Format(FRHelpers.DragonProfileUrl, id));

                if (dragonProfileDoc.GetElementbyId("error-404") != null)
                {
                    await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                    {
                        Content = "This dragon does not exist",
                        Flags = MessageFlags.EPHEMERAL
                    });

                    return;
                }

                var webhookResponse = new DiscordWebhookFilesRequest();
                webhookResponse.PayloadJson.Content = "";

                var isExalted = dragonProfileDoc.GetElementbyId("exalted-content") != null;

                var embed = new DiscordEmbed
                {
                    Url = string.Format(FRHelpers.DragonProfileUrl, id),
                    Thumbnail = new DiscordEmbedThumbnail { Url = $"attachment://{id}_350.png" },
                    Image = new DiscordEmbedImage { Url = $"attachment://{id}_350.png" }
                };
                var fields = new List<DiscordEmbedField>();

                if (isExalted)
                {
                    var name = dragonProfileDoc.DocumentNode.QuerySelector(".breadcrumbs").ChildNodes.Last().InnerHtml;
                    var exaltedToFlight = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""exalted-content""]/div/div[2]/div/p[1]/strong[2]").InnerHtml;

                    embed.Title = name;
                    embed.Description = $"**{name}** has been exalted to the {FRHelpers.GetFlightFromGodName(exaltedToFlight)} flight.\nThe only info available is lineage.";

                    var parentsField = new DiscordEmbedField { Name = "Parents", Inline = true };
                    var siblingsField = new DiscordEmbedField { Name = "Offspring", Inline = true };

                    var parentsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""exalted-content""]/div/div[2]/div/fieldset/div/ul[1]").SelectNodes("li");
                    var siblingsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""exalted-content""]/div/div[2]/div/fieldset/div/ul[2]").SelectNodes("li");

                    if (parentsNodes[0].FirstChild.Name == "em")
                        parentsField.Value = "_none_";
                    else
                        parentsField.Value = string.Join("\n", parentsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));

                    if (siblingsNodes[0].FirstChild.Name == "em")
                        siblingsField.Value = "_none_";
                    else
                        if (siblingsNodes.Count <= 5)
                        siblingsField.Value = string.Join("\n", siblingsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));
                    else
                        siblingsField.Value = string.Join("\n", siblingsNodes.Take(5).Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})")) + $"\n_{siblingsNodes.Count - 5} more.._";

                    fields.Add(parentsField);
                    fields.Add(siblingsField);
                }
                else
                {
                    var name = dragonProfileDoc.DocumentNode.QuerySelector(".dragon-profile-header-name").InnerHtml;
                    var ownerNode = dragonProfileDoc.DocumentNode.QuerySelector(".breadcrumbs").ChildNodes.First();
                    var level = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-battle""]/div[1]/strong").InnerText.Replace("&infin;", "∞");

                    var eggbreakday = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[1]/div/div/div[2]/strong").InnerText;
                    var dragonAge = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[2]/div/div/div[2]").ChildNodes[0].InnerText.Replace("\\n", "").Trim().ToLower();
                    var dragonBreed = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[2]/div/div/div[2]/strong").InnerText;

                    var badges = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-secondary""]/div[2]/div[3]").ChildNodes.Nodes().Where(x => x.Name == "img");
                    var gender = badges.First(x => x.ParentNode.Attributes.FirstOrDefault(a => a.Name == "data-tooltip-source")?.Value == "#dragon-profile-icon-sex-tooltip").Attributes["src"].Value.EndsWith("female.png") ? "female" : "male";
                    var permaBaby = badges.Any(x => x.Attributes["src"].Value.EndsWith("eternal-youth.png"));

                    embed.Title = name;
                    embed.Description = $"**{name}** is a level {level}, {(permaBaby ? "**eternal youth**" : dragonAge)} {gender} {dragonBreed} in [{ownerNode.InnerHtml}]({ownerNode.Attributes["href"].Value})'{(ownerNode.InnerHtml.EndsWith("s") ? "" : "s")} lair.";

                    var eyeColor = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[3]/div/div/div[2]").ChildNodes[0].InnerText.Replace("\\n", "").Trim();
                    var eyeType = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-physical""]/div/div[2]/div[3]/div/div/div[2]/strong").InnerText;

                    fields.Add(new DiscordEmbedField { Name = "General info", Value = $"**Eye type:** {eyeColor} {eyeType}\n**Eggday:** {eggbreakday}\n**ID:** {id}", Inline = true });

                    var frPrimaryGene = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-primary-gene""]/strong").InnerText.Trim();
                    var frPrimaryColor = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-primary-gene""]").FirstChild.InnerText.Trim();
                    var frSecondaryGene = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-secondary-gene""]/strong").InnerText.Trim();
                    var frSecondaryColor = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-secondary-gene""]").FirstChild.InnerText.Trim();
                    var frTertiaryGene = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-tertiary-gene""]/strong").InnerText.Trim();
                    var frTertiaryColor = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-tertiary-gene""]").FirstChild.InnerText.Trim();

                    var primaryGeneSanitized = frPrimaryGene.Split(' ').First();
                    var secondaryGeneSanitized = frSecondaryGene.Split(' ').First();
                    var tertiaryGeneSanitized = frTertiaryGene.Split(' ').First();

                    var dragonType = (DragonType)Enum.Parse(typeof(DragonType), dragonBreed);

                    fields.Add(new DiscordEmbedField { Name = "Genetics", Value = $"**Primary:** {frPrimaryColor} {frPrimaryGene}\n**Secondary:** {frSecondaryColor} {frSecondaryGene}\n**Tertiary:** {frTertiaryColor} {frTertiaryGene}\n", Inline = true });

                    var parentsField = new DiscordEmbedField { Name = "Parents", Inline = true };
                    var siblingsField = new DiscordEmbedField { Name = "Offspring", Inline = true };

                    if (dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-details-lineage""]/div/div") != null)
                    {
                        fields.Add(new DiscordEmbedField { Name = "God", Value = "This is one of the **eleven elemental deities**; it has no parents or offspring.", Inline = true });
                    }
                    else
                    {
                        var parentsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-details-lineage""]/div/ul[1]").SelectNodes("li");
                        var siblingsNodes = dragonProfileDoc.DocumentNode.SelectSingleNode(@"//*[@id=""dragon-profile-details-lineage""]/div/ul[2]").SelectNodes("li");

                        if (parentsNodes[0].FirstChild.Name == "em")
                            parentsField.Value = "_none_";
                        else
                            parentsField.Value = string.Join("\n", parentsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));

                        if (siblingsNodes[0].FirstChild.Name == "em")
                            siblingsField.Value = "_none_";
                        else
                        {
                            if (siblingsNodes.Count <= 5)
                                siblingsField.Value = string.Join("\n", siblingsNodes.Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})"));
                            else
                                siblingsField.Value = string.Join("\n", siblingsNodes.Take(5).Select(x => $"[{x.FirstChild.InnerHtml}]({x.FirstChild.Attributes["href"].Value})")) + $"\n_{siblingsNodes.Count - 5} more.._";
                        }

                        fields.Add(parentsField);
                        fields.Add(siblingsField);
                    }
                }
                embed.Fields = fields;

                var dragonImage = await _itemAssetDataService.GetDragonRender(id.Value);
                webhookResponse.Files = new Dictionary<string, byte[]> { { $"{id}_350.png", dragonImage } };

                webhookResponse.PayloadJson.Embeds = new List<DiscordEmbed> { embed };

                await DiscordService.EditInitialInteraction(interaction.Token, webhookResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await DiscordService.EditInitialInteraction(interaction.Token, new DiscordWebhookRequest
                {
                    Content = "Something went wrong parsing the dragon's profile page, maybe something changed on the page and I need to be updated. Please let <@107155889563115520> know!",
                    Flags = MessageFlags.EPHEMERAL
                });

                return;
            }
        }

        private long? GetIdFromInteraction(DiscordInteractionRequest interaction)
        {
            long? id = 0;
            var input = interaction.Data.Options.First().Options.First();
            if (input.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
                id = jsonElement.Deserialize<long>();
            else if (input.Name == "url" && input.Value is string url)
            {
                var urlParse = Regex.Match(url, @"/(?<id>\d+)");
                if (urlParse.Success)
                    id = long.Parse(urlParse.Groups["id"].Value);
            }

            return id;
        }
    }
}