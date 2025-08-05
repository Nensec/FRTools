using FRTools.Core.Common.Extentions;
using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.Discord.DiscordModels.MessageModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Services
{
    public class DiscordGuildService : IDiscordGuildService
    {
        private const string GUILDMEMBERURL = "https://discord.com/api/v10/guilds/{0}/members/{1}";

        private readonly ILogger<DiscordGuildService> _logger;

        public DiscordGuildService(ILogger<DiscordGuildService> logger)
        {
            _logger = logger;
        }

        public async Task<Message?> ModifyGuildMember(ulong guildId, Member guildMember) => await SendPatchRequest(guildMember, string.Format(GUILDMEMBERURL, guildId, guildMember.User.Id));
        public async Task<Member?> GetGuildMember(ulong guildId, ulong userId) => await SendGetRequest<Member>(string.Format(GUILDMEMBERURL, guildId, userId));

        private async Task<Message?> SendPatchRequest(Member guildMember, string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));

                var response = await client.PatchAsJsonAsync(url, guildMember);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format("Error modifying guild member, response code: {0}\\n\\tUrl: {1}\\n\\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(guildMember)), null, response.StatusCode);
                }

                return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<T?> SendGetRequest<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format("Error modifying guild member, response code: {0}\\n\\tUrl: {1}", response.StatusCode, url), null, response.StatusCode);
                }

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}