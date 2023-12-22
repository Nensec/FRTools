using FRTools.Core.Services.Announce;

namespace FRTools.Core.Services.Interfaces
{
    public interface IAnnounceService
    {
        void RegisterAnnouncer(IAnnouncer announcer);
        Task Announce(AnnounceData announceData);
    }
}