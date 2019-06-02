using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web;

namespace FRSkinTester.Models
{
    public class BaseSkinModel
    {
        public string CDNBasePath => ConfigurationManager.AppSettings["CDNBasePath"];

        public adnmaster.Bitly.BitlyClient BitlyClient { get; } = new adnmaster.Bitly.BitlyClient(ConfigurationManager.AppSettings["BitlyClientId"]);

        public BaseSkinModel() => BitlyClient.ApplyAccessToken(ConfigurationManager.AppSettings["BitlyAT"]);
        public string TryGenerateUrl(string url)
        {
            // bitly cannot create shortened links for localhost
            if (url.Contains("localhost"))
                return url;

            var result = BitlyClient.Links.Shorten(url);
            if(result.StatusCode != System.Net.HttpStatusCode.OK)            
                return url;
            return result.Data.ShortUrl;
        }
    }

    public class UploadModelPost : BaseSkinModel
    {
        [Display(Name = "Name of your skin")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Dragon type")]
        public DragonType DragonType { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "Skin file")]
        [Required]
        public HttpPostedFileBase Skin { get; set; }
    }

    public class UploadModelPostViewModel : BaseSkinModel
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class PreviewModelGet : BaseSkinModel
    {
        public string SkinId { get; set; }
    }

    public class PreviewModelViewModel : BaseSkinModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string SkinId { get; set; }
        public string PreviewUrl { get; set; }
        public double? Coverage { get; set; }
        public User Creator { get; set; }
    }

    [SmartRequired]
    public partial class PreviewModelPost : BaseSkinModel
    {
        public PreviewModelPost(string skinId) => SkinId = skinId;
        public PreviewModelPost() { }

        [IgnoreRequired]
        public string SkinId { get; set; }

        [Display(Name = "Dragon ID")]
        public int? DragonId { get; set; }
        [Display(Name = "Scry image URL")]
        public string ScryerUrl { get; set; }
        [Display(Name = "Dressing image URL")]
        public string DressingRoomUrl { get; set; }

        [IgnoreRequired]
        public bool Force { get; set; }
    }

    public class PreviewModelPostViewModel : BaseSkinModel
    {
        public PreviewResult Result { get; set; }
        public string SkinId { get; set; }
        public DragonCache Dragon { get; set; }
    }

    public class DeleteSkinPost : BaseSkinModel
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class ManageModelGet : BaseSkinModel
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class ManageModelViewModel : BaseSkinModel
    {
        public Skin Skin { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class ManageModelPost : BaseSkinModel
    {
        [Display(Name = "Name of your skin")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Dragon type")]
        public DragonType DragonType { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }
}