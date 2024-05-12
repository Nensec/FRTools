using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class StaticWebFunctions : FunctionBase
    {
        private readonly ILogger<StaticWebFunctions> _logger;

        public StaticWebFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StaticWebFunctions>();
        }

        [Function(nameof(WebRoot))]
        public async Task<HttpResponseData> WebRoot([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "/")] HttpRequestData request)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return await OkResult(request, "Testing root functionality, pun intended");
        }
    }
}
