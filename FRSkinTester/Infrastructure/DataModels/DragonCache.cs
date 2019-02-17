using FRSkinTester.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FRSkinTester.Infrastructure.DataModels
{
    public class DragonCache
    {
        [Key]
        public int Id { get; set; }

        public DragonType DragonType { get; set; }
        public Gender Gender { get; set; }
        public BodyGene BodyGene { get; set; }
        public Color BodyColor { get; set; }
        public WingGene WingGene { get; set; }
        public Color WingColor { get; set; }
        public TertiaryGene TertiaryGene { get; set; }
        public Color TertiaryColor { get; set; }
        public EyeType EyeType { get; set; }
        public Element Element { get; set; }
        [StringLength(40)]
        [Index]
        public string SHA1Hash { get; set; }

        public static DragonCache FromString(string data)
        {
            var split = data.Split('_');
            var dragon = new DragonCache
            {
                Gender = (Gender)int.Parse(split[0]),
                DragonType = (DragonType)int.Parse(split[1])
            };

            if (split.Length > 2)
                dragon.Element = (Element)int.Parse(split[2]);
            if (split.Length > 3)
                dragon.EyeType = (EyeType)int.Parse(split[3]);
            if (split.Length > 4)
                dragon.BodyGene = (BodyGene)int.Parse(split[4]);
            if (split.Length > 5)
                dragon.WingGene = (WingGene)int.Parse(split[5]);
            if (split.Length > 6)
                dragon.TertiaryGene = (TertiaryGene)int.Parse(split[6]);
            if (split.Length > 7)
                dragon.BodyColor = (Color)int.Parse(split[7]);
            if (split.Length > 8)
                dragon.WingColor = (Color)int.Parse(split[8]);
            if (split.Length > 9)
                dragon.TertiaryColor = (Color)int.Parse(split[9]);

            return dragon;
        }

        public string GenerateAzureUrl() => $@"dragoncache\{SHA1Hash}.png";

        public override string ToString() => $"{(int)Gender}_{(int)DragonType}_{(int)Element}_{(int)EyeType}_{(int)BodyGene}_{(int)WingGene}_{(int)TertiaryGene}_{(int)BodyColor}_{(int)WingColor}_{(int)TertiaryColor}";
    }
}