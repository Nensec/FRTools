using DontPanic.TumblrSharp;
using FRTools.Core.Data;
using FRTools.Core.Services.Interfaces;

namespace FRTools.Core.Services.Announce.Announcers.TumblrAnnouncers
{
    public interface ITumbleDominanceAnnouncer
    {
        Task AnnounceDominance(DominanceAnnounceData data);
    }

    public class TumbleDominanceAnnouncer : ITumbleDominanceAnnouncer
    {
        private readonly ITumblrService _tumblrService;

        public TumbleDominanceAnnouncer(ITumblrService tumblrService)
        {
            _tumblrService = tumblrService;
        }

        public async Task AnnounceDominance(DominanceAnnounceData data)
        {
            var tags = new List<string> { "frtools", "fr tools", "flight rising", "flightrising", "fr", "dominance", data.Flights[0].ToString(), data.Flights[1].ToString(), data.Flights[2].ToString() };
            var body = $"<p>Dominance has been calculated and the winner of this week is <b>{data.Flights[0]}</b>!</p>";
            body += "<p>The top 3 standings were as follows:";
            body += "<ol>";
            body += $"<li>{data.Flights[0]}</li>";
            body += $"<li>{data.Flights[1]}</li>";
            body += $"<li>{data.Flights[2]}</li>";
            body += "</ol></p>";

            if (data.Flights[0] != Flight.Beastclans)
            {
                body += "<p>First place gets a nice 15% discount on the treasure market place and a 5% discount on lair upgrades. Additionally, they get +1500 treasure a day and +3 gathering turns.</p>";
            }
            else
            {
                body += "<p>Wait.. Beastclans won!? Why did Earth not win at least..? Alright.. well, instead of first place we'll just list the second place benefits then I suppose..<br/>";
                body += "Second place gets a nice 7% discount on the treasure market place and a 1% discount on lair upgrades..<br/>";
                body += "They also get +750 treasure a day and +2 gathering turns..</p>";
            }

            var post = PostData.CreateText(body, $"Congratulations to {data.Flights[0]}!", tags);

            await _tumblrService.CreatePost("frtools", post);
        }

    }
}
