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
}