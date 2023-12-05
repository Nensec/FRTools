namespace FRTools.Core.Data
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
