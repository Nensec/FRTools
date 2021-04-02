using FRTools.Data.DataModels;
using FRTools.Data.DataModels.PinglistModels;
using FRTools.Tools.SkinTester;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace FRTools.Web.Models
{
    public class ViewProfileViewModel
    {
        public User User { get; set; }
        public List<Preview> Previews { get; set; } = new List<Preview>();
        public List<Skin> Skins { get; set; } = new List<Skin>();
        public List<Pinglist> Pinglists { get; set; } = new List<Pinglist>();
        public string CDNBasePath => ConfigurationManager.AppSettings["CDNBasePath"];
        public bool IsOwn { get; set; }
        public async Task<string> GetDummyPreviewImage(string skinId, int version)
        {
            return (await SkinTester.GenerateOrFetchDummyPreview(skinId, version)).Urls[0];
        }
    }


    public class EditProfileViewModel : ProfileSettings
    {
        public string Username { get; set; }
    }
}