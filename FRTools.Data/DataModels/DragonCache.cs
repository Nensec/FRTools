﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FRTools.Data.DataModels
{
    public class DragonCache
    {
        [Key]
        public int Id { get; set; }

        public DragonType DragonType { get; set; }
        public Gender Gender { get; set; }
        public int BodyGene { get; set; }
        public Color BodyColor { get; set; }
        public int WingGene { get; set; }
        public Color WingColor { get; set; }
        public int TertiaryGene { get; set; }
        public Color TertiaryColor { get; set; }
        public EyeType EyeType { get; set; }
        public Element Element { get; set; }
        public Age Age { get; set; }
        public string Apparel { get; set; }
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
                dragon.BodyGene = (int)Enum.Parse(dragon.DragonType == DragonType.Gaoler ? typeof(GaolerBodyGene) : typeof(BodyGene), split[4]);
            if (split.Length > 5)
                dragon.WingGene = (int)Enum.Parse(dragon.DragonType == DragonType.Gaoler ? typeof(GaolerWingGene) : typeof(WingGene), split[5]);
            if (split.Length > 6)
                dragon.TertiaryGene = (int)Enum.Parse(dragon.DragonType == DragonType.Gaoler ? typeof(GaolerTertGene) : typeof(TertiaryGene), split[6]);
            if (split.Length > 7)
                dragon.BodyColor = (Color)int.Parse(split[7]);
            if (split.Length > 8)
                dragon.WingColor = (Color)int.Parse(split[8]);
            if (split.Length > 9)
                dragon.TertiaryColor = (Color)int.Parse(split[9]);
            if (split.Length > 10)
                dragon.Age = (Age)int.Parse(split[10]);
            if (split.Length > 11)
                dragon.Apparel = split[11].Replace('-', ',');
            return dragon;
        }

        public string ConstructImageString()
        {
            if (SHA1Hash != null && SHA1Hash != "dummy")
            {
                return $"http://www1.flightrising.com/dgen/preview/dragon?age={(int)Age}&body={(int)BodyColor}&bodygene={BodyGene}&breed={(int)DragonType}&element={(int)Element}&eyetype={(int)EyeType}&gender={(int)Gender}&tert={(int)TertiaryColor}&tertgene={TertiaryGene}&winggene={WingGene}&wings={(int)WingColor}&auth={SHA1Hash}&dummyext=prev.png";
            }
            else
                throw new Exception($"SHA1Hash '{SHA1Hash}' cannot be used to construct an image url");
        }

        public override string ToString() => $"{(int)Gender}_{(int)DragonType}_{(int)Element}_{(int)EyeType}_{Convert.ToInt32(BodyGene)}_{Convert.ToInt32(WingGene)}_{Convert.ToInt32(TertiaryGene)}_{(int)BodyColor}_{(int)WingColor}_{(int)TertiaryColor}_{(int)Age}_{Apparel?.Replace(',', '-') ?? ""}";

        public void SetApparel(int[] apparelIds) => Apparel = string.Join(",", apparelIds);

        public int[] GetApparel() => string.IsNullOrEmpty(Apparel) ? new int[] { } : Apparel.Split(',').Select(x => int.Parse(x)).ToArray();

        public Enum GetBodyGene() => (Enum)Enum.Parse(DragonType == DragonType.Gaoler ? typeof(GaolerBodyGene) : typeof(BodyGene), BodyGene.ToString());
        public Enum GetWingGene() => (Enum)Enum.Parse(DragonType == DragonType.Gaoler ? typeof(GaolerWingGene) : typeof(WingGene), WingGene.ToString());
        public Enum GetTertGene() => (Enum)Enum.Parse(DragonType == DragonType.Gaoler ? typeof(GaolerTertGene) : typeof(TertiaryGene), TertiaryGene.ToString());
    }
}