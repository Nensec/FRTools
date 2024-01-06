namespace FRTools.Core.Services.Announce
{
    public interface IAnnouncer
    {
        Task Announce(AnnounceData announceData);
    }

    public interface IDominanceAnnouncer : IAnnouncer { }
    public interface IFlashSaleAnnouncer : IAnnouncer { }
    public interface INewItemAnnouncer : IAnnouncer { }
}