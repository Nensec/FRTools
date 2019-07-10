namespace FRTools.Web.Infrastructure
{
    public class PreviewResult
    {
        public string[] Urls { get; set; }
        public bool Forced { get; set; }
        public bool Cached { get; set; }
        public string RealDragon { get; set; }
        public string DressingRoomUrl { get; set; }
        public string Exception { get; set; }
    }
}