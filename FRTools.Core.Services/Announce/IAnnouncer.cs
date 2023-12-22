namespace FRTools.Core.Services.Announce
{
    public interface IAnnouncer
    {
        Task Announce(AnnounceData announceData);
    }
}
