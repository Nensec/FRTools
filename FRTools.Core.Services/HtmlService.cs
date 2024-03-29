﻿using System.Net;
using System.Text;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace FRTools.Core.Services
{
    public class HtmlService : IHtmlService
    {
        public async Task<HtmlDocument> LoadHtmlPage(string url)
        {
            var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

            var response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Found && response.Headers.Location != null)
            {
                return await LoadHtmlPage(response.Headers.Location.AbsoluteUri);
            }

            var resultDocument = new HtmlDocument();
            resultDocument.Load(response.Content.ReadAsStream(), Encoding.GetEncoding("utf-8"));

            return resultDocument;
        }
    }
}
