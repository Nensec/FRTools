using FRTools.Core.Services.Announce.Announcers.TumblrAnnouncers;

namespace FRTools.Core.Services.Announce.Announcers
{
    public class TumblrAnnouncer : IDominanceAnnouncer, IFlashSaleAnnouncer
    {
        private readonly ITumbleDominanceAnnouncer _tumbleDominanceAnnouncer;
        private readonly ITumblrFlashSaleAnnouncer _tumblrFlashSaleAnnouncer;

        public TumblrAnnouncer(ITumbleDominanceAnnouncer tumbleDominanceAnnouncer, ITumblrFlashSaleAnnouncer tumblrFlashSaleAnnouncer)
        {

            _tumbleDominanceAnnouncer = tumbleDominanceAnnouncer;
            _tumblrFlashSaleAnnouncer = tumblrFlashSaleAnnouncer;
        }

        public async Task Announce(AnnounceData announceData)
        {
            switch (announceData)
            {
                case DominanceAnnounceData dominanceAnnounceData:
                    await _tumbleDominanceAnnouncer.AnnounceDominance(dominanceAnnounceData);
                    break;
                case FlashSaleAnnounceData flashSaleAnnounceData:
                    await _tumblrFlashSaleAnnouncer.AnnounceFlashSale(flashSaleAnnounceData);
                    break;
            }
        }
    }
}
