using FRTools.Discord.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Discord.Metadata
{
    class Test
    {
        public Test()
        {
            var botAssembly = Assembly.GetAssembly(typeof(BaseModule));
            var types = botAssembly.GetTypes().Where(x => x.BaseType == typeof(BaseModule));
            foreach(var module in types)
            {
                var attr = module.GetCustomAttributes().First();
                attr.GetType().GetProperty("Text").GetValue(attr);
                var methods = module.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                foreach(var method in methods)
                {
                    var attributes = method.GetCustomAttributes();
                }
            }
        }
    }
}
