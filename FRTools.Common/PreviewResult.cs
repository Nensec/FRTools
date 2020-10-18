using FRTools.Data.DataModels;
using FRTools.Data.DataModels.FlightRisingModels;

namespace FRTools.Common
{
    public class PreviewResult
    {
        public string[] Urls { get; set; }
        public bool Forced { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public bool Cached { get; set; }
        public string DragonUrl { get; set; }
        public DragonCache Dragon { get; set; }
        public Skin Skin { get; set; }
    }
}