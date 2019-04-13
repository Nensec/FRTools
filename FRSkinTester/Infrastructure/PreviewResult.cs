using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRSkinTester.Infrastructure
{
    public class PreviewResult
    {
        public string[] Urls { get; set; }
        public bool Forced { get; set; }
        public bool Cached { get; set; }
        public string RealDragon { get; set; }
        public string DressingRoomUrl { get; set; }
    }
}