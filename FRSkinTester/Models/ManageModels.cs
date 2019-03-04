using FRSkinTester.Infrastructure.DataModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace FRSkinTester.Models
{
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

    public class UserViewModel
    {
        public User User { get; set; }
    }

    public class UserPostViewModel
    {
        public string User_Username { get; set; }
        public string User_Email { get; set; }
    }
}