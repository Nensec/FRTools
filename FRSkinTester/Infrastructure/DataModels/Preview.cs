using System;

namespace FRTools.Infrastructure.DataModels
{
    public class Preview
    {
        public int Id { get; set; }
        public Skin Skin { get; set; }
        public int? DragonId { get; set; }
        public string ScryerUrl { get; set; }
        public string PreviewImage { get; set; }
        public string DragonData { get; set; }
        public DateTime? PreviewTime { get; set; }
        public int Version { get; set; }

        public User Requestor { get; set; }
    }
}