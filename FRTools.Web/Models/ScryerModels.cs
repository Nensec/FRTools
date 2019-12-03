using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace FRTools.Web.Models
{
    public class DressModelViewModel
    {
        [Display(Name = "Scry image URL")]
        [Required]
        public string ScryerUrl { get; set; }
        [Display(Name = "Dressing image URL")]
        [Required]
        public string DressingRoomUrl { get; set; }
    }

    public class DressModelResultViewModel
    {
        public string PreviewUrl { get; set; }
        public string CDNBasePath => ConfigurationManager.AppSettings["CDNBasePath"];
    }
}