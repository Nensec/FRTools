using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSec.Cryptography;

namespace FRTools.Core.Functions.Workers
{
    public class DiscordInteractionFunctions : FunctionBase
    {
        private readonly IDiscordRequestService _discordService;
        private readonly ILogger<DiscordInteractionFunctions> _logger;

        public DiscordInteractionFunctions(IDiscordRequestService discordService, ILogger<DiscordInteractionFunctions> logger)
        {
            _discordService = discordService;
            _logger = logger;
        }

        [Function(nameof(DiscordInteractionEndpoint))]
        public async Task<HttpResponseData> DiscordInteractionEndpoint([HttpTrigger(AuthorizationLevel.Function, "post", Route = "discord")] HttpRequestData request)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var body = await GetBody(request);

            if (!CheckSecurity(request.Headers.GetValues("X-Signature-Ed25519").FirstOrDefault(), request.Headers.GetValues("X-Signature-Timestamp").FirstOrDefault(), body))
                return request.CreateResponse(HttpStatusCode.Unauthorized);

            _logger.LogInformation($"Command received:\n{body}");

            try
            {
                var interactionData = JsonConvert.DeserializeObject<DiscordInteractionRequest>(body)!;

                if (interactionData.Type == InteractionType.PING)
                {
                    _logger.LogInformation("Ping found, returning pong.");
                    return await JsonResult(request, new DiscordInteractionResponse.PongResponse());
                }

                var discordResponse = await _discordService.ExecuteInteraction(interactionData);

                return await JsonResult(request, discordResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong sending interaction response. Request {0}", body);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Function(nameof(RegisterCommands))]
        public async Task<HttpResponseData> RegisterCommands([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "discord/registerCommands")] HttpRequestData request)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await _discordService.RegisterAllCommands();
            return request.CreateResponse(HttpStatusCode.OK);
        }

        [Function(nameof(ProcessCommand))]
        public async Task ProcessCommand([ServiceBusTrigger("%AzureServiceBusCommandQueue%", AutoCompleteMessages = true, Connection = "AZURESBCONNSTR_defaultConnection")] DiscordInteractionRequest interaction)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed command: {interaction.Data.Name}");

            await _discordService.ExecuteDeferedInteraction(interaction);
        }

        private bool CheckSecurity(string? signature, string? timestamp, string body)
        {
            if (signature == null)
                return false;

            if (timestamp == null)
                return false;

            _logger.LogInformation("Checking security for request.");
            var algorithm = SignatureAlgorithm.Ed25519;

            var publicKeyBytes = Convert.FromHexString(Environment.GetEnvironmentVariable("DiscordPublicKey")!);
            var publicKey = PublicKey.Import(algorithm, publicKeyBytes, KeyBlobFormat.RawPublicKey);

            var signatureBytes = Convert.FromHexString(signature);

            var result = algorithm.Verify(publicKey, Encoding.UTF8.GetBytes(timestamp + body), signatureBytes);
            _logger.LogInformation($"Result checking security: {result}.");

            return result;
        }

        private async Task<string> GetBody(HttpRequestData request)
        {
            using (var reader = new StreamReader(request.Body))
                return await reader.ReadToEndAsync();
        }
    }
}
