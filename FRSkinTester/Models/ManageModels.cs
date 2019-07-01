using FRTools.Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace FRTools.Models
{
    public class BaseManageModel
    {
        public string CDNBasePath => ConfigurationManager.AppSettings["CDNBasePath"];
    }

    public class AccountViewModel : BaseManageModel
    {
        public User User { get; set; }
        public List<Skin> Skins { get; set; }
    }

    public class VerifyFRViewModel : BaseManageModel
    {
        public Guid Guid { get; set; }
    }

    public class VerifyFRPostViewModel : BaseManageModel
    {
        [Display(Name = "User/Lair Id")]
        [Required]
        public int LairId { get; set; }
    }

    public class UserPostViewModel : BaseManageModel
    {
        [Display(Name = "Username"), StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Profile visibility")]
        public Privacy Privacy { get; set; }
        [Display(Name = "Default skin visibility")]
        public SkinVisiblity DefaultVisibility { get; set; }
    }
}