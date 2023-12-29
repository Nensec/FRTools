using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels;
using FRTools.Core.Services.Discord.DiscordModels.InteractionResponseModels;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSec.Cryptography;

namespace FRTools.Core.Functions.Workers
{
    public class DiscordInteractionFunctions : FunctionBase
    {
        private readonly IHandleDiscordRequestService _discordService;

        public DiscordInteractionFunctions(IHandleDiscordRequestService discordService)
        {
            _discordService = discordService;
        }

        [FunctionName(nameof(DiscordInteractionEndpoint))]
        public async Task<IActionResult> DiscordInteractionEndpoint([HttpTrigger(AuthorizationLevel.Function, "post", Route = "discord")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var body = GetBody(req);

            if (!CheckSecurity(req.Headers["X-Signature-Ed25519"], req.Headers["X-Signature-Timestamp"], body, log))
                return new UnauthorizedResult();

            log.LogInformation($"Command received:\n{body}");

            try
            {
                var interactionData = JsonConvert.DeserializeObject<DiscordInteractionRequest>(body);

                if (interactionData.Type == InteractionType.PING)
                {
                    log.LogInformation("Ping found, returning pong.");
                    return AckResult();
                }

                var response = await _discordService.ExecuteInteraction(interactionData);

                var responseContent = JsonConvert.SerializeObject(response);
                log.LogInformation("Sending response: {0}", responseContent);

                return new ContentResult { Content = responseContent, ContentType = "application/json", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Something went wrong sending interaction response. Request {0}", body);
            }

            return new BadRequestResult();
        }

        [FunctionName(nameof(RegisterCommands))]
        public async Task<IActionResult> RegisterCommands([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "discord/registerCommands")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _discordService.RegisterAllCommands();
            return new OkResult();
        }

        [FunctionName(nameof(ProcessCommand))]
        public async Task ProcessCommand([ServiceBusTrigger("%AzureServiceBusCommandQueue%", AutoCompleteMessages = true, Connection = "AZURESBCONNSTR_defaultConnection")] DiscordInteractionRequest interaction, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed command: {interaction.Data.Name}");

            await _discordService.ExecuteDeferedInteraction(interaction);
        }

        private bool CheckSecurity(string signature, string timestamp, string body, ILogger log)
        {
            log.LogInformation("Checking security for request.");
            var algorithm = SignatureAlgorithm.Ed25519;

            var publicKeyBytes = Convert.FromHexString(Environment.GetEnvironmentVariable("DiscordPublicKey"));
            var publicKey = PublicKey.Import(algorithm, publicKeyBytes, KeyBlobFormat.RawPublicKey);

            var signatureBytes = Convert.FromHexString(signature);

            var result = algorithm.Verify(publicKey, Encoding.UTF8.GetBytes(timestamp + body), signatureBytes);
            log.LogInformation($"Result checking security: {result}.");

            return result;
        }

        private IActionResult AckResult() => new ContentResult { Content = JsonConvert.SerializeObject(new DiscordInteractionResponse.PongResponse()), ContentType = "application/json", StatusCode = 200 };

        private string GetBody(HttpRequest req)
        {
            using (var reader = new StreamReader(req.Body))
                return reader.ReadToEnd();
        }
    }
}
