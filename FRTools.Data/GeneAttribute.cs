using System;

namespace FRTools.Data
{
    public class GeneAttribute : Attribute
    {
        public DragonType[] ValidFor { get; set; }

        public GeneAttribute(params DragonType[] dragonTypes)
        {
            ValidFor = dragonTypes;
        }
    }
}
