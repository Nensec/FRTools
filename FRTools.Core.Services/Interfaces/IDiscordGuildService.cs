using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.Discord.DiscordModels.MessageModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IDiscordGuildService
    {
        Task<Member?> GetGuildMember(ulong guildId, ulong userId);
        Task<Message?> ModifyGuildMember(ulong guildId, Member guildMember);
    }
}