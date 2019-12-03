using Discord.Addons.Interactive;
using Discord.Commands;
using FRTools.Data;
using FRTools.Data.DataModels.DiscordModels;
using FRTools.Discord.Infrastructure;
using System.Linq;

namespace FRTools.Discord.Modules
{
    public class BaseModule : InteractiveBase
    {
        protected DataContext DbContext { get; }
        protected SettingManager SettingManager { get; }
        protected DiscordServer Server { get; private set; }
        protected DiscordChannel Channel { get; private set; }

        public BaseModule(DataContext dbContext, SettingManager settingManager)
        {
            DbContext = dbContext;
            SettingManager = settingManager;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            if (Context.Guild != null)
            {
                Server = DbContext.DiscordServers.FirstOrDefault(x => x.ServerId == (long)Context.Guild.Id) ?? DbContext.DiscordServers.Add(new DiscordServer { ServerId = (long)Context.Guild.Id, Name = Context.Guild.Name });
                Channel = DbContext.DiscordChannels.FirstOrDefault(x => x.ChannelId == (long)Context.Channel.Id) ?? DbContext.DiscordChannels.Add(new DiscordChannel { Server = Server, ChannelId = (long)Context.Channel.Id, Name = Context.Channel.Name });
                DbContext.SaveChanges();
            }

            base.BeforeExecute(command);
        }

        protected override void AfterExecute(CommandInfo command)
        {
            if (!command.Attributes.Any(x => x is NoLogAttribute) && !command.Module.Attributes.Any(x => x is NoLogAttribute))
            {
                DbContext.DiscordLogs.Add(new DiscordLog { Channel = Channel, UserId = (long)Context.User.Id, Module = command.Module.Name, Command = command.Name, Data = Context.Message.Content });
                DbContext.SaveChanges();
            }

            base.AfterExecute(command);
        }
    }
}
