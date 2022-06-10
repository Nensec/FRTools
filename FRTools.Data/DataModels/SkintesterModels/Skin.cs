﻿using System.Collections.Generic;

namespace FRTools.Data.DataModels
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
        public double? Coverage { get; set; }
        public SkinVisiblity Visibility { get; set; }
        public int Version { get; set; } = 1;
        public virtual User Creator { get; set; }

        public virtual List<Preview> Previews { get; set; } = new List<Preview>();
    }
}