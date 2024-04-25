using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class StaticWebFunctions : FunctionBase
    {
        private readonly ILogger<StaticWebFunctions> _logger;

        public StaticWebFunctions(ILogger<StaticWebFunctions> logger)
        {
            _logger = logger;
        }

        [Function(nameof(WebRoot))]
        public IActionResult WebRoot([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "site")] HttpRequest request)
        {
            return new OkObjectResult("Testing root functionality, pun intended");
        }
    }
}
