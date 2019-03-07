using FRSkinTester.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Age Age { get; set; }
        [StringLength(40)]
        [Index]
        public string SHA1Hash { get; set; }

        public string GenerateAzureUrl() => $@"dragoncache\{SHA1Hash}.png";

        public override string ToString() => $"{(int)Gender}_{(int)DragonType}_{(int)Element}_{(int)EyeType}_{(int)BodyGene}_{(int)WingGene}_{(int)TertiaryGene}_{(int)BodyColor}_{(int)WingColor}_{(int)TertiaryColor}_{(int)Age}";
    }
}