using System.Collections.Generic;

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
        public double? Coverage { get; set; }
        public SkinVisiblity Visibility { get; set; }

        public virtual User Creator { get; set; }

        public virtual List<Preview> Previews { get; set; } = new List<Preview>();
    }
}