using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class DominanceTrackerFunction : FunctionBase
    {
        private readonly DataContext _dataContext;

        public DominanceTrackerFunction(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [FunctionName(nameof(DominanceTracker))]
        public async Task DominanceTracker([TimerTrigger("30 0 9 * * 0", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            while (true)
            {
                try
                {
                    var dominanceHtml = await Common.Helpers.LoadHtmlPage("https://www1.flightrising.com/dominance");
                    var domTexts = dominanceHtml.DocumentNode.QuerySelectorAll("#domtext > div");

                    Flight[] positions = new Flight[3];
                    for (var i = 0; i <= 2; i++)
                    {
                        positions[i] = Enum.Parse<Flight>(domTexts[i].QuerySelector(".domglow").InnerText);
                        log.LogInformation($"{i + 1}: {positions[i]}");
                    }

                    var previousDom = _dataContext.FRDominances.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                    if (previousDom == null || previousDom.First != (int)positions[0] || previousDom.Second != (int)positions[1] || previousDom.Third != (int)positions[2])
                    {
                        _dataContext.FRDominances.Add(new FRDominance { Timestamp = DateTime.UtcNow, First = (int)positions[0], Second = (int)positions[1], Third = (int)positions[2] });
                        await _dataContext.SaveChangesAsync();

                        using (var tumblrClient = new TumblrClientFactory().Create<TumblrClient>(
                            Environment.GetEnvironmentVariable("TumblrClientId"),
                            Environment.GetEnvironmentVariable("TumblrSecret"),
                            new DontPanic.TumblrSharp.OAuth.Token(
                            Environment.GetEnvironmentVariable("TumblrOAuthToken"),
                            Environment.GetEnvironmentVariable("TumblrOAuthSecret"))))
                        {
                            var body = $"<p>Dominance has been calculated and the winner of this week is <b>{positions[0]}</b>!</p>";
                            body += "<p>The top 3 standings were as follows:";
                            body += "<ol>";
                            body += $"<li>{positions[0]}</li>";
                            body += $"<li>{positions[1]}</li>";
                            body += $"<li>{positions[2]}</li>";
                            body += "</ol></p>";

                            if (positions[0] != Flight.Beastclans)
                            {
                                body += "<p>First place gets a nice 15% discount on the treasure market place and a 5% discount on lair upgrades. Additionally, they get +1500 treasure a day and +3 gathering turns.</p>";
                            }
                            else
                            {
                                body += "<p>Wait.. Beastclans won!? Why did Earth not win at least..? Alright.. well, instead of first place we'll just list the second place benefits then I suppose..<br/>";
                                body += "Second place gets a nice 7% discount on the treasure market place and a 1% discount on lair upgrades..<br/>";
                                body += "They also get +750 treasure a day and +2 gathering turns..</p>";
                            }

                            var tags = new List<string> { "frtools", "fr tools", "flight rising", "flightrising", "fr", "dominance", positions[0].ToString(), positions[1].ToString(), positions[2].ToString() };

                            var post = PostData.CreateText(body, $"Congratulations to {positions[0]}!", tags);
                            if (DEBUG)
                                post.State = PostCreationState.Private;
                            await tumblrClient.CreatePostAsync("frtools", post);
                        }
                        break;
                    }
                    else
                    {
                        log.LogWarning("Results were the same, not updated yet?");
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex.ToString());
                }
                log.LogInformation("Waiting 30 seconds to try again..");
                await Task.Delay(30000);
            }
        }
    }
}
