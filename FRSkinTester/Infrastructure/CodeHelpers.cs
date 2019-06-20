using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FRTools
{
    public static class CodeHelpers
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            return fi.GetCustomAttribute<DescriptionAttribute>()?.Description ?? fi.GetCustomAttribute<DisplayAttribute>()?.GetDescription() ?? value.ToString();
        }
    }
}