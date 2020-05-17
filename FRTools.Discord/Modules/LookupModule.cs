using Discord.Commands;
using FRTools.Common;
using FRTools.Data;
using FRTools.Discord.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace FRTools.Discord.Modules
{
    [Name("Lookup"), Group("lookup"), Alias("lu"), Summary("Lookup related commands")]
    [DiscordHelp("LookupModule")]
    public class LookupModule : BaseModule
    {
        public LookupModule(DataContext dbContext, SettingManager settingManager) : base(dbContext, settingManager)
        {
        }

        [Command("dragon"), Name("Dragon"), Alias("d")]
        [DiscordHelp("LookupDragon")]
        public async Task DragonLookup(int id)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var dragonPage = await client.DownloadStringTaskAsync(string.Format(FRHelpers.DragonProfileUrl, id));

                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(dragonPage);

                    var level = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[2]/div/div[1]");
                    var dragon = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[2]/div/div[2]");
                    var hatchdat = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[2]/div/div[4]");

                    var STR = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[1]/span[2]");
                    var INT = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[2]/span[2]");
                    var AGI = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[3]/span[2]");
                    var VIT = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[4]/span[2]");
                    var DEF = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[5]/span[2]");
                    var MND = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[6]/span[2]");
                    var QCK = xmlDoc.SelectSingleNode("/html/body/div[1]/div[2]/div[2]/div/div[3]/div[2]/fieldset/div[2]/span[4]/div/a[7]/span[2]");
                }
                catch
                {
                    // Something went poopy
                }
            }
        }

        [Command("skin"), Name("Skin"), Alias("accent", "a", "s")]
        [DiscordHelp("LookupSkin")]
        public async Task SkinLookup(int id)
        {

        }

        [Command("familiar"), Name("Familiar"), Alias("f")]
        [DiscordHelp("LookupFamiliar")]
        public async Task FamiliarLookup(int id)
        {
            // https://www1.flightrising.com/hoard/preview-image?breed=14&gender=0&item=551
            // https://flightrising.com/includes/itemajax.php?id=551&tab=equipment
        }
    }
}