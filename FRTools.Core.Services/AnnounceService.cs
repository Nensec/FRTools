﻿using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services
{
    public class AnnounceService : IAnnounceService
    {
        private readonly ILogger<AnnounceService> _logger;
        private List<IAnnouncer> _announcers = new List<IAnnouncer>();

        public AnnounceService(ILogger<AnnounceService> logger)
        {
            _logger = logger;
        }

        public async Task Announce(AnnounceData announceData)
        {
            foreach(var announcer in _announcers)
            {
                try
                {
                    await announcer.Announce(announceData);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong attempting to announce {0} using {1}", announceData.GetType().Name, announcer.GetType().Name);
                }
            }
        }

        public void RegisterAnnouncer(IAnnouncer announcer)
        {
            _announcers.Add(announcer);
        }
    }
}
