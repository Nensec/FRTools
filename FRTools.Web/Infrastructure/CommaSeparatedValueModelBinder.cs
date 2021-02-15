using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Web.Infrastructure
{
    public class CommaSeparatedValueModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);

            var listProperties = bindingContext.ModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(List<>));

            foreach (var listProperty in listProperties)
            {
                var csv = controllerContext.HttpContext.Request.QueryString[listProperty.Name];

                if (csv != null)
                {
                    if (string.IsNullOrEmpty(csv))
                    {
                        listProperty.SetValue(model, Activator.CreateInstance(listProperty.PropertyType));
                    }
                    else
                    {
                        var values = JsonConvert.SerializeObject(csv.Split(','));
                        listProperty.SetValue(model, JsonConvert.DeserializeObject(values, listProperty.PropertyType));
                    }
                }
            }

            return model;
        }
    }
}