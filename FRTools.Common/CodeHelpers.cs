using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
            if(server != null)            
                return HttpContext.Current.Server.MapPath("\\bin" + path);

            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + path;
        }        
    }
}