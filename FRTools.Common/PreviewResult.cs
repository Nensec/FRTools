using FRTools.Data.DataModels;
using FRTools.Data.DataModels.FlightRisingModels;
using System.Linq;

namespace FRTools.Common
{
    public class PreviewResult
    {
        public string[] Urls { get; set; }
        public bool Forced { get; set; }
        public bool Success { get; set; }
        public bool Cached { get; set; }
        public string DragonUrl { get; set; }
        public DragonCache Dragon { get; set; }
        public Skin Skin { get; set; }

        private string _errorMessage;
        private object[] _errorMessageArgs;
        public PreviewResult WithErrorMessage(string format, params object[] args)
        {
            _errorMessage = format;
            _errorMessageArgs = args;
            return this;
        }

        public string GetDiscordErrorMessage => string.Format(_errorMessage, _errorMessageArgs.Select(x => $"**{x}**").ToArray());
        public string GetHtmlErrorMessage => string.Format(_errorMessage, _errorMessageArgs.Select(x => $"<b>{x}</b>").ToArray());
    }
}