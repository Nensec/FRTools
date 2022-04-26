using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace FRTools.Web.Infrastructure
{
    public class BotAuthenticator : IAuthenticator
    {
        string _authHeader;

        public BotAuthenticator(string botToken) => SetBearerToken(botToken);

        public void SetBearerToken(string botToken) => _authHeader = $"Bot {botToken}";

        public ValueTask Authenticate(RestClient client, RestRequest request) => new ValueTask(Task.FromResult(request.AddOrUpdateParameter("Authorization", _authHeader, ParameterType.HttpHeader)));
    }
}