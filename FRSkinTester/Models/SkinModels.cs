using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FRSkinTester.Models
{
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

    public class UploadModelPostViewModel
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class PreviewModelGet
    {
        public string SkinId { get; set; }
    }

    public class PreviewModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string SkinId { get; set; }
        public string PreviewUrl { get; set; }
        public double? Coverage { get; set; }
        public User Creator { get; set; }
    }

    public class PreviewModelPost : PreviewModelBase
    {
        [Display(Name = "Your dragon id")]
        [Required]
        public int DragonId { get; set; }
        public bool Force { get; set; }
    }

    public class PreviewScryerModelPost : PreviewModelBase
    {
        [Display(Name = "Scry image URL")]
        [Required]
        public string ScryerUrl { get; set; }
    }

    public class PreviewModelPostViewModel
    {
        public PreviewResult Result { get; set; }
        public string SkinId { get; set; }
        public DragonCache Dragon { get; set; }
    }

    public class DeleteSkinPost
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class ManageModelGet
    {
        public string SkinId { get; set; }
        public string SecretKey { get; set; }
    }

    public class ManageModelViewModel
    {
        public Skin Skin { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class ManageModelPost
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