using FRTools.Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace FRTools.Web.Models
{
    public class BaseManageModel
    {
        public string CDNBasePath => ConfigurationManager.AppSettings["CDNBasePath"];
    }

    public class AccountViewModel : BaseManageModel
    {
        public User User { get; set; }
    }

    public class VerifyFRViewModel : BaseManageModel
    {
        public Guid Guid { get; set; }
    }

    public class VerifyFRPostViewModel : BaseManageModel
    {
        [Required]
        public int LairId { get; set; }
    }

    public class UserPostViewModel : BaseManageModel
    {
        [StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }
        public string Email { get; set; }
        public bool ShowFRLinkStatus { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLogin> CurrentLogins { get; set; }
        public bool ShowRemoveButton { get; set; }
    }
}