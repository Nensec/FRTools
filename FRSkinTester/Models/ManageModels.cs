using FRSkinTester.Infrastructure.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRSkinTester.Models
{
    public class AccountViewModel
    {
        public User User { get; set; }
        public List<Skin> Skins { get; set; }
        public string CDNBasePath { get; set; }
    }

    public class VerifyFRViewModel
    {
        public Guid Guid { get; set; }
    }

    public class VerifyFRPostViewModel
    {
        [Display(Name = "User/Lair Id")]
        [Required]
        public int LairId { get; set; }
    }

    public class UserPostViewModel
    {
        [Display(Name = "Username"), StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ClaimSkinPostViewModel
    {
        [Display(Name = "Skin Id")]
        public string SkinId { get; set; }
        [Display(Name = "Secret Key")]
        public string SecretKey { get; set; }
    }
}