using System;
using System.Linq;
using FRTools.Data;

namespace FRTools.MS.LogCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new DataContext())
            {
                var minDate = DateTime.UtcNow.AddDays(-30).Date;
                Console.WriteLine($"Checking for log messages older than {minDate:dd/MM/yyyy}");
                var oldLogs = ctx.DiscordLogs.Where(x => x.CreatedAt == null || minDate > x.CreatedAt).ToList();
                Console.WriteLine($"Found {oldLogs.Count} old logs");
                if (oldLogs.Count == 0)
                    return;
                Console.WriteLine("Removing old logs");
                ctx.DiscordLogs.RemoveRange(oldLogs);
                ctx.SaveChanges();
            }
        }
    }
}
