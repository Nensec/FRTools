using HtmlAgilityPack;

namespace FRTools.Core.Services.Interfaces
{
    public interface IHtmlService
    {
        Task<HtmlDocument> LoadHtmlPage(string url);
    }
}