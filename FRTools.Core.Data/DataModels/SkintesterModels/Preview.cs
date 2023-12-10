using FRTools.Core.Data.DataModels.AccountModels;

namespace FRTools.Core.Data.DataModels
{
    public class Preview
    {
        public int Id { get; set; }
        public virtual Skin Skin { get; set; }
        public int? DragonId { get; set; }
        public string ScryerUrl { get; set; }
        public string PreviewImage { get; set; }
        public string DragonData { get; set; }
        public DateTime? PreviewTime { get; set; }
        public int Version { get; set; }

        public virtual User Requestor { get; set; }
    }
}