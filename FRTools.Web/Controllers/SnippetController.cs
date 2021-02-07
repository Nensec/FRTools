using FRTools.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("snippets")]
    public class SnippetController : BaseController
    {
        public static Dictionary<string, BaseScriptModel> Scripts = new Dictionary<string, BaseScriptModel>();

        static SnippetController()
        {
            var dir = Directory.GetFiles(string.Format("{0}/Views/Snippet/Scripts", HostingEnvironment.ApplicationPhysicalPath), "*.cshtml", SearchOption.AllDirectories);

            foreach (var view in dir)
            {
                var viewPath = view.Replace(HostingEnvironment.ApplicationPhysicalPath, string.Empty);
                var modelProperty = BuildManager.GetCompiledType(viewPath).GetProperties().FirstOrDefault(p => p.Name == "Model");

                if (modelProperty.PropertyType.BaseType == typeof(BaseScriptModel))
                {
                    Scripts.Add(viewPath, Activator.CreateInstance(modelProperty.PropertyType) as BaseScriptModel);
                }
            }
        }

        [Route("", Name = "SnippetsHome")]
        public ActionResult Index() => View(Scripts);
    }
}