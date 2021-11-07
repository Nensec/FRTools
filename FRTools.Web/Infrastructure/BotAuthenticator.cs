using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Validation;

namespace FRTools.Web.Infrastructure
{
    public class BotAuthenticator : IAuthenticator
    {
        string _authHeader;

        public BotAuthenticator(string botToken) => SetBearerToken(botToken);

        public void SetBearerToken(string botToken)
        {
            Ensure.NotEmpty(botToken, nameof(botToken));

            _authHeader = $"Bot {botToken}";
        }

        public void Authenticate(IRestClient client, IRestRequest request) => request.AddOrUpdateParameter("Authorization", _authHeader, ParameterType.HttpHeader);
    }
}