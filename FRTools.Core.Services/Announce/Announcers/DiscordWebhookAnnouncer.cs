using FRTools.Core.Services.Announce.Announcers.DiscordAnnouncers;

namespace FRTools.Core.Services.Announce.Announcers
{
    public class DiscordWebhookAnnouncer : IFlashSaleAnnouncer, IDominanceAnnouncer, INewItemAnnouncer
    {
        private readonly IDiscordFlashSaleAnnouncer _discordFlashSaleAnnouncer;
        private readonly IDiscordDominanceAnnouncer _discordDominanceAnnouncer;
        private readonly IDiscordNewItemsAnnouncer _discordNewItemsAnnouncer;

        public DiscordWebhookAnnouncer(IDiscordFlashSaleAnnouncer discordFlashSaleAnnouncer, IDiscordDominanceAnnouncer discordDominanceAnnouncer, IDiscordNewItemsAnnouncer discordNewItemsAnnouncer)
        {
            _discordFlashSaleAnnouncer = discordFlashSaleAnnouncer;
            _discordDominanceAnnouncer = discordDominanceAnnouncer;
            _discordNewItemsAnnouncer = discordNewItemsAnnouncer;
        }

        public Task Announce(AnnounceData announceData)
        {
            switch (announceData)
            {
                case FlashSaleAnnounceData flashSaleData:
                    return _discordFlashSaleAnnouncer.AnnounceFlashSale(flashSaleData);
                case DominanceAnnounceData dominanceAnnounceData:
                    return _discordDominanceAnnouncer.AnnounceDominance(dominanceAnnounceData);
                case NewItemsAnnounceData newItemsData:
                    return _discordNewItemsAnnouncer.AnnounceNewItems(newItemsData);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}