using System;
using System.Linq;
using System.Threading.Tasks;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;

namespace FRTools.Core.Functions.Workers
{
    public class DominanceTrackerFunction : FunctionBase
    {
        private readonly DataContext _dataContext;
        private readonly IHtmlService _htmlService;
        private readonly IAnnounceService _announceService;
        private readonly ILogger<DominanceTrackerFunction> _logger;

        public DominanceTrackerFunction(DataContext dataContext, IHtmlService htmlService, IAnnounceService announceService, ILogger<DominanceTrackerFunction> logger)
        {
            _dataContext = dataContext;
            _htmlService = htmlService;
            _announceService = announceService;
            _logger = logger;
        }

        [Function(nameof(DominanceTracker))]
        public async Task DominanceTracker([TimerTrigger("30 0 9 * * 0", RunOnStartup = DEBUG)] TimerInfo timer)
        {
            while (true)
            {
                try
                {
                    var dominanceHtml = await _htmlService.LoadHtmlPage("https://www1.flightrising.com/dominance");
                    var domTexts = dominanceHtml.DocumentNode.QuerySelectorAll("#domtext > div");

                    Flight[] positions = new Flight[3];
                    for (var i = 0; i <= 2; i++)
                    {
                        positions[i] = Enum.Parse<Flight>(domTexts[i].QuerySelector(".domglow").InnerText);
                        _logger.LogInformation($"{i + 1}: {positions[i]}");
                    }

                    var previousDom = _dataContext.FRDominances.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                    if (previousDom == null || previousDom.First != (int)positions[0] || previousDom.Second != (int)positions[1] || previousDom.Third != (int)positions[2])
                    {
                        _dataContext.FRDominances.Add(new FRDominance { Timestamp = DateTime.UtcNow, First = (int)positions[0], Second = (int)positions[1], Third = (int)positions[2] });
                        await _dataContext.SaveChangesAsync();

                        await _announceService.Announce(new DominanceAnnounceData(positions));
                        break;
                    }
                    else
                    {
                        _logger.LogWarning("Results were the same, not updated yet?");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
                _logger.LogInformation("Waiting 30 seconds to try again..");
                await Task.Delay(30000);
            }
        }
    }
}
