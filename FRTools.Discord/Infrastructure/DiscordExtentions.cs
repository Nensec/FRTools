using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
