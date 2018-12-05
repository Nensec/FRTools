using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRSkinTester.Infrastructure.DataModels
{
    public class Skin
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DragonType { get; set; }
        public int GenderType { get; set; }
        public string GeneratedId { get; set; }
        public string SecretKey { get; set; }
    }
}