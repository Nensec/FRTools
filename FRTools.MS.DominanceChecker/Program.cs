using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FRTools.Common;
using FRTools.Common.Jobs;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.Messages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.MS.DominanceChecker
{
    class Program
    {
        private static IQueueClient _serviceBus;

        static async Task Main()
        {
            _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.DominanceTracker, "Started")))));
            while (true)
            {
                try
                {
                    Flight[] positions = new Flight[3];
                    for (var i = 0; i <= 2; i++)
                    {
                        var request = (HttpWebRequest)WebRequest.Create("https://flightrising.com/includes/ol/dom_text.php");
                        var data = Encoding.ASCII.GetBytes($"position={i + 1}");
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                            stream.Write(data, 0, data.Length);

                        var response = (HttpWebResponse)request.GetResponse();

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var responseString = streamReader.ReadToEnd();

                            var parse = Regex.Match(responseString, @"<span[\s\S]*>([a-zA-Z]+)</span>");
                            positions[i] = (Flight)Enum.Parse(typeof(Flight), parse.Groups[1].Value);
                            Console.WriteLine($"Position: {i + 1} = {positions[i]}");
                        }
                    }
                    using (var ctx = new DataContext())
                    {
                        var previousDom = ctx.FRDominances.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                        if (previousDom == null || previousDom.First != (int)positions[0] || previousDom.Second != (int)positions[1] || previousDom.Third != (int)positions[2])
                        {
                            ctx.FRDominances.Add(new FRDominance { Timestamp = DateTime.UtcNow, First = (int)positions[0], Second = (int)positions[1], Third = (int)positions[2] });
                            await ctx.SaveChangesAsync();

                            using (var tumblrClient = new TumblrClientFactory().Create<TumblrClient>(
                            ConfigurationManager.AppSettings["TumblrClientId"],
                            ConfigurationManager.AppSettings["TumblrSecret"],
                            new Token(
                            ConfigurationManager.AppSettings["TumblrOAuthToken"],
                            ConfigurationManager.AppSettings["TumblrOAuthSecret"])))
                            {
                                var body = $"Dominance has been calculated and the winner of this week is <b>{positions[0]}</b>!";

                                body += "</br>";
                                body += "</br>";
                                body += "The top 3 standings were as follows:";
                                body += "<ol>";
                                body += $"<li>{positions[0]}</li>";
                                body += $"<li>{positions[1]}</li>";
                                body += $"<li>{positions[2]}</li>";
                                body += "</ol>";

                                body += "</br>";
                                body += "</br>";

                                if (positions[0] != Flight.Beastclans)
                                {
                                    body += "First place gets a nice 15% discount on the treasure market place and a 5% discount on lair upgrades.";
                                    body += "</br>Additionally, they get +1500 treasure a day and +3 gathering turns.";
                                }
                                else
                                {
                                    body += "Wait.. Beastclans won!? Why did Earth not win at least..?";
                                    body += "</br>Alright.. well, instead of first place we'll just list the second place benefits then I suppose..";

                                    body += "</br>";
                                    body += "</br>";

                                    body += "Second place gets a nice 7% discount on the treasure market place and a 1% discount on lair upgrades..";
                                    body += "</br>They also get +750 treasure a day and +2 gathering turns..";
                                }

                                var tags = new List<string> { "frtools", "fr tools", "flight rising", "flightrising", "fr", "dominance", positions[0].ToString(), positions[1].ToString(), positions[2].ToString() };

                                var post = PostData.CreateText(body, $"Congratulations to {positions[0]}!", tags);
                                await tumblrClient.CreatePostAsync("frtools", post);
                            }

                            var (JobId, StartTime, Task) = JobManager.StartNewJob(new SendUpdateDominanceMessage());
                            while (Task == null || !Task.IsCompleted)
                                await Task.Delay(100);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Results were the same, not updated yet?");
                            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.DominanceTracker, "Same result")))));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine("Waiting 30 seconds to try again..");
                await Task.Delay(30000);
            }
            await _serviceBus.CloseAsync();
        }
    }
}
