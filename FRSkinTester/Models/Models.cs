using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FRSkinTester.Models
{
    public enum DragonType
    {
        Fae = 1,
        Guardian,
        Mirror,
        Pearlcatcher,
        Ridgeback,
        Tundra,
        Spiral,
        Imperial,
        Snapper,
        Wildclaw,
        Nocturne,
        Coatl,
        Skydancer,
        Bogsneak,
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class UploadModelPost
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

    public class UploadModelPostResult
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class PreviewModelGet
    {
        public string SkinId { get; set; }
    }

    public class PreviewModelPost
    {
        [Display(Name = "Your dragon id")]
        [Required]
        public int DragonId { get; set; }
        public string SkinId { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class PreviewModelPostResult
    {
        public string ImageResultUrl { get; set; }
        public string SkinId { get; set; }
    }

    public class DeleteSkinPost
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }
}