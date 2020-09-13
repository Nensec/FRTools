using System;
using System.Linq;

namespace FRTools.Data
{
    public class ColorAttribute : Attribute
    {
        public string ColorHexCode { get; }

        public ColorAttribute(string colorHexCode)
        {
            ColorHexCode = colorHexCode;
        }
    }

    public static class ColorHelper
    {
        public static string GetHex(this Color color) => (typeof(Color).GetMember(color.ToString()).FirstOrDefault()?.GetCustomAttributes(typeof(ColorAttribute), false).FirstOrDefault() as ColorAttribute).ColorHexCode;
    }
}
