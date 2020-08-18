using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FRTools
{
    public static class CodeHelpers
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            return fi.GetCustomAttribute<DescriptionAttribute>()?.Description ?? fi.GetCustomAttribute<DisplayAttribute>()?.GetDescription() ?? value.ToString();
        }

        public static string MapPath(string path)
        {
            var server = HttpContext.Current;
            if (server != null)
                return HttpContext.Current.Server.MapPath("\\bin" + path);

            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + path;
        }

        private static Random _random = new Random(Guid.NewGuid().GetHashCode());

        public static string GenerateId(int length = 7, IEnumerable<string> mustNotMatch = null)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id = "";
            for (int i = 0; i < length; i++)
                id += chars.Skip(_random.Next(chars.Length)).First();
            if (mustNotMatch?.Contains(id) == true)
            {
                return GenerateId(length, mustNotMatch);
            }
            return id;
        }
    }
}