using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discord
{
    public static class DiscordExtentions
    {
        public static async Task DelayedDelete(this IMessage message, TimeSpan delay)
        {
            await Task.Delay(delay);
            await message.DeleteAsync();
        }

        public static LogLevel ToLogLevel(this LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Critical: return LogLevel.Critical;
                case LogSeverity.Error: return LogLevel.Error;
                case LogSeverity.Warning: return LogLevel.Warning;
                case LogSeverity.Info: return LogLevel.Information;
                default: return LogLevel.Trace;
            }
        }
    }
}
