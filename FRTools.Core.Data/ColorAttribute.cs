namespace FRTools.Core.Data
{
    public class ColorAttribute : Attribute
    {
        public string ColorHexCode { get; }
        public int FROrder { get; }

        public ColorAttribute(string colorHexCode, int frOrder)
        {
            ColorHexCode = colorHexCode;
            FROrder = frOrder;
        }
    }

    public static class ColorHelper
    {
        public static string GetHex(this Color color) => (typeof(Color).GetMember(color.ToString()).FirstOrDefault()?.GetCustomAttributes(typeof(ColorAttribute), false).FirstOrDefault() as ColorAttribute).ColorHexCode;
        public static int FROrder(this Color color) => (typeof(Color).GetMember(color.ToString()).FirstOrDefault()?.GetCustomAttributes(typeof(ColorAttribute), false).FirstOrDefault() as ColorAttribute).FROrder;
    }
}
