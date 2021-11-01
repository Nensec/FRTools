using System.ComponentModel.DataAnnotations;

namespace FRTools.Web.Models
{
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class DeleteAccountViewModel
    {
        [Required]
        public bool Confirm { get; set; }
    }
}