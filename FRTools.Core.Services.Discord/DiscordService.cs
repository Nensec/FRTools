using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using FRTools.Core.Data;
using FRTools.Core.Services.Discord.Commands;
using FRTools.Core.Services.DiscordModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Services
{
    public class DiscordService : IDiscordService
    {
        private string _commandsUrl = $"https://discord.com/api/v10/applications/{Environment.GetEnvironmentVariable("DiscordApplicationId")}/commands";

        private readonly DataContext _dataContext;
        private readonly ILogger<DiscordService> _logger;

        private List<DiscordCommand> _commands;

        public DiscordService(DataContext dataContext, IServiceProvider serviceProvider, ILogger<DiscordService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;

            var commands = Assembly.GetAssembly(typeof(DiscordCommand))!.GetTypes().Where(x => typeof(DiscordCommand).IsAssignableFrom(x)).ToList();
            _commands = commands.Select(x => (DiscordCommand)serviceProvider.GetService(x)!).ToList();
        }

        public AppCommand? ParseJson(string json) => JsonConvert.DeserializeObject<AppCommand>(json);

        public async Task RegisterCommand(AppCommand command)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));
                await client.PostAsJsonAsync(_commandsUrl, command);
            }
        }

        public async Task RegisterAllCommands()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));
                await client.PutAsJsonAsync(_commandsUrl, _commands.Select(x => x.Command).ToArray());
            }
        }

        public async Task ExecuteInteraction(AppCommand command)
        {
            await _commands.First(x => x.CommandName == command.name).ExecuteAsync(command);
        }

        public async Task ReplyToInteraction(AppCommand command)
        {

        }

        public async Task DeferReplyToInteraction(AppCommand command)
        {

        }
    }
}
