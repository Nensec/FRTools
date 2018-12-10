using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRSkinTester.Infrastructure.DataModels
{
    public class Preview
    {
        public int Id { get; set; }
        public Skin Skin { get; set; }
        public int? DragonId { get; set; }
        public string ScryerUrl { get; set; }
        public string PreviewImage { get; set; }
        public string DragonData { get; set; }
    }
}