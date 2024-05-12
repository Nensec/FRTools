using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace FRTools.Core.Functions.Workers
{
    public abstract class FunctionBase
    {
#if DEBUG
        protected const bool DEBUG = true;
#else
        protected const bool DEBUG = false;
#endif

        protected Task<HttpResponseData> OkResult(HttpRequestData request) => OkResult(request, null);

        protected async Task<HttpResponseData> OkResult(HttpRequestData request, string? content)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (content != null)
                await response.WriteStringAsync(content);

            return response;
        }

        protected async Task<HttpResponseData> JsonResult<T>(HttpRequestData request, T content)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(JsonConvert.SerializeObject(content));

            return response;
        }
        protected async Task<HttpResponseData> FileResult(HttpRequestData request, byte[] bytes, string contentType = "image/png")
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", contentType);
            await response.WriteBytesAsync(bytes);

            return response;
        }
    }
}
