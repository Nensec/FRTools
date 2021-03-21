using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FRTools.Data;
using FRTools.Data.DataModels.DiscordModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FRTools.Discord.Handlers
{
    public static class UserHandler
    {
        public static async Task SyncServer(SocketGuild guild, SocketCommandContext context = null)
        {
            Console.WriteLine($"Started sync for {guild.Name}");
            await guild.DownloadUsersAsync();

            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.FirstOrDefault(x => x.ServerId == (long)guild.Id);
                if (dbServer == null)
                {
                    ctx.DiscordServers.Add(dbServer = new DiscordServer());
                    dbServer.ServerId = (long)guild.Id;
                }
                dbServer.Name = guild.Name;

                if (guild.IconUrl != null)
                {
                    using (var client = new WebClient())
                    {
                        var iconData = client.DownloadData(guild.IconUrl);
                        dbServer.IconBase64 = Convert.ToBase64String(iconData);
                    }
                }
                ctx.SaveChanges();
            }

            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.Include(x => x.Roles).FirstOrDefault(x => x.ServerId == (long)guild.Id);

                foreach (var existingRole in dbServer.Roles.ToList())
                {
                    if (!guild.Roles.Any(x => (long)x.Id == existingRole.RoleId))
                        ctx.DiscordRoles.Remove(existingRole);
                }

                foreach (var role in guild.Roles)
                {
                    var dbRole = dbServer.Roles.FirstOrDefault(x => x.RoleId == (long)role.Id);
                    if (dbRole == null)
                    {
                        dbServer.Roles.Add(dbRole = new DiscordRole());
                        dbRole.RoleId = (long)role.Id;
                    }
                    dbRole.Name = role.Name;
                    dbRole.Color = role.Color.RawValue.ToString();
                    dbRole.DiscordPermissions = (long)role.Permissions.RawValue;
                }
                var msg = $"Saving roles.. {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Count()} new roles, {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count()} deleted roles, {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Count()} changed roles. Server has {dbServer.Roles.Count()} roles total.";
                if (context != null)
                {
                    await context.Channel.SendMessageAsync(msg);
                }
                Console.WriteLine(msg);
                ctx.SaveChanges();
            }

            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.Include(x => x.Channels).FirstOrDefault(x => x.ServerId == (long)guild.Id);

                foreach (var existingChannel in dbServer.Channels.ToList())
                {
                    if (!guild.Channels.Any(x => (long)x.Id == existingChannel.ChannelId))
                        ctx.DiscordChannels.Remove(existingChannel);
                }

                foreach (var channel in guild.Channels)
                {
                    var dbChannel = dbServer.Channels.FirstOrDefault(x => x.ChannelId == (long)channel.Id);
                    if (dbChannel == null)
                    {
                        dbServer.Channels.Add(dbChannel = new DiscordChannel());
                        dbChannel.ChannelId = (long)channel.Id;
                    }
                    dbChannel.ChannelType = channel is ITextChannel ? DiscordChannelType.Text : channel is IVoiceChannel ? DiscordChannelType.Voice : channel is ICategoryChannel ? DiscordChannelType.Category : DiscordChannelType.Other;
                    dbChannel.Name = channel.Name;
                }
                var msg = $"Saving channels.. {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Count()} new channels, {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count()} deleted channels, {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Count()} changed channels. Server has {dbServer.Channels.Count()} channels total.";
                if (context != null)
                {
                    await context.Channel.SendMessageAsync(msg);
                }
                Console.WriteLine(msg);
                ctx.SaveChanges();
            }

            using (var ctx = new DataContext())
            {
                var dbServer = ctx.DiscordServers.Include(x => x.Users).FirstOrDefault(x => x.ServerId == (long)guild.Id);

                foreach (var user in guild.Users)
                {
                    var dbServerUser = dbServer.Users.FirstOrDefault(x => x.User.UserId == (long)user.Id);
                    if (dbServerUser == null)
                    {
                        var dbUser = ctx.DiscordUsers.FirstOrDefault(x => x.UserId == (long)user.Id);
                        if (dbUser == null)
                            ctx.DiscordUsers.Add(dbUser = new DiscordUser { UserId = (long)user.Id });
                        dbServer.Users.Add(dbServerUser = new DiscordServerUser { User = dbUser });
                    }
                    dbServerUser.Nickname = user.Nickname;
                    dbServerUser.Roles.Clear();
                    dbServerUser.Roles = dbServer.Roles.Where(x => user.Roles.Any(r => (long)r.Id == x.RoleId)).ToList();
                    dbServerUser.User.Username = user.Username;
                    dbServerUser.User.Discriminator = user.DiscriminatorValue;
                    dbServerUser.IsOwner = guild.OwnerId == user.Id;
                }

                var msg = $"Saving users.. {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Count()} new users, {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count()} deleted users, {ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Count()} changed users. Server has {dbServer.Users.Count()} users total.";
                if (context != null)
                {
                    await context.Channel.SendMessageAsync(msg);
                }
                Console.WriteLine(msg);
                ctx.SaveChanges();
            }
            Console.WriteLine($"Completed sync for {guild.Name}");
        }
    }
}
