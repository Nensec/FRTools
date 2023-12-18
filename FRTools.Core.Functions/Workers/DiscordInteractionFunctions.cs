using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IDiscordService _discordService;

        public DiscordInteractionFunctions(IDiscordService discordService)
        {
            _discordService = discordService;
        }

        [FunctionName(nameof(DiscordInteractionEndpoint))]
        public IActionResult DiscordInteractionEndpoint([HttpTrigger(AuthorizationLevel.Function, "post", Route = "discord")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var body = GetBody(req);

            if (!CheckSecurity(req.Headers["X-Signature-Ed25519"], req.Headers["X-Signature-Timestamp"], body, log))
                return new UnauthorizedResult();

            if (CheckPing(body, log))
                return AckResult();

            return new BadRequestResult();
        }

        [FunctionName(nameof(RegisterCommands))]
        public async Task<IActionResult> RegisterCommands([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "discord/registerCommands")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _discordService.RegisterAllCommands();
            return new OkResult();
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

        private bool CheckPing(string body, ILogger log)
        {
            log.LogInformation("Checking for ping in body.");

            var simpleParse = JsonConvert.DeserializeObject<dynamic>(body);
            try
            {
                if (simpleParse.type == 1)
                {
                    log.LogInformation("Ping found.");
                    return true;
                }
            }
            catch { }

            log.LogInformation("No ping found.");

            return false;
        }

        private IActionResult AckResult() => new ContentResult { Content = JsonConvert.SerializeObject(new { type = 1 }), ContentType = "application/json", StatusCode = 200 };

        private string GetBody(HttpRequest req)
        {
            using (var reader = new StreamReader(req.Body))
                return reader.ReadToEnd();
        }
    }
}
