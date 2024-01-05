using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace FRTools.Core.Functions.Workers
{
    public class StaticWebFunctions : FunctionBase
    {
        [FunctionName(nameof(WebRoot))]
        public IActionResult WebRoot([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "/")] HttpRequest request)
        {
            return new ContentResult() { Content = "Testing root functionality, pun intended" };
        }
    }
}
