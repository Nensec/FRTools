using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace FRTools.Core.Services
{
    public class FRUserService : IFRUserService
    {
        private readonly DataContext _dataContext;

        public FRUserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<FRUser> GetOrUpdateFRUser(string username, DataContext _dataContext = null) => await GetOrUpdateFRUser(username, userId: null);
        public async Task<FRUser> GetOrUpdateFRUser(int userId, DataContext _dataContext = null) => await GetOrUpdateFRUser(null, userId);

        private async Task<FRUser> GetOrUpdateFRUser(string username, int? userId)
        {
            Console.WriteLine($"Checking if we know {username ?? userId.ToString()}");

            var frUser = _dataContext.FRUsers.FirstOrDefault(x => x.Username == username || x.FRId == userId);

            if (frUser == null)
            {
                Console.WriteLine("We do not, fetching user info");
                var (frName, frId) = await GetFRUserInfo(username, userId);

                if (frName == null)
                {
                    var prev = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("User cannot be found");
                    Console.ForegroundColor = prev;
                    return null;
                }

                frUser = _dataContext.FRUsers.FirstOrDefault(x => x.Username == frName || x.FRId == frId);
                if (frUser == default)
                {
                    frUser = new FRUser();
                    _dataContext.FRUsers.Add(frUser);
                }
                frUser.Username = frName;
                frUser.FRId = frId;
                frUser.LastUpdated = DateTime.UtcNow;

                await _dataContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("We do, updating user");
                frUser = await UpdateFRUser(frUser);
            }

            await _dataContext.SaveChangesAsync();

            return frUser;
        }


        public async Task<FRUser> UpdateFRUser(FRUser frUser)
        {
            // Only update if it hasn't been a day to avoid spamming FR server
            if (DateTime.UtcNow < frUser.LastUpdated.AddDays(1))
            {
                Console.WriteLine("User recently updated, skipping");
                return frUser;
            }

            var (frName, frId) = await GetFRUserInfo(null, frUser.FRId);

            if (frName == null)
                return null;

            frUser.Username = frName;
            frUser.FRId = frId;
            frUser.LastUpdated = DateTime.UtcNow;
            await Task.Delay(50);
            return frUser;
        }

        private static async Task<(string Username, int UserId)> GetFRUserInfo(string username, int? userId)
        {
            string url = $"https://www1.flightrising.com/clan-profile/{(userId?.ToString() ?? $"n/{username}")}";
            var client = new HtmlWeb();
            var userProfilePage = client.Load(url);

            if (userProfilePage.ParsedText.Contains("404 - Page Not Found") || userProfilePage.ParsedText.Contains("404: User not found"))
                return default;

            var userBio = userProfilePage.DocumentNode.QuerySelector(".clan-profile-user-frame");
            var frId = int.Parse(userBio.QuerySelectorAll(".clan-profile-stats .clan-profile-stat").First(x => x.QuerySelector("strong").InnerText == "Player ID").LastChild.InnerText.Trim());
            var frName = userBio.QuerySelector(".clan-profile-username").InnerText.Trim();
            Console.WriteLine($"Found info:\n\tUsername: {frName}\n\tID: {frId}");

            return (frName, frId);
        }
    }
}
