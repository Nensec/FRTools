using FRTools.Data.DataModels;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.DataModels.PinglistModels;
using FRTools.Web.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRTools.Web.Models
{
    public class PinglistViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Public list")]
        public bool IsPublic { get; set; }
        [Display(Name = "List ID")]
        public string ListId { get; set; }
        [Display(Name = "Secret key")]
        public string SecretKey { get; set; }
        public PinglistEntriesViewModel EntriesViewModel { get; set; }
        public FRUser CurrentFRUser { get; set; }
        public User Owner { get; set; }
        public PinglistCategory PinglistCategory { get; set; }
        public string CopyPinglist { get; set; }
    }

    public class CreatePinglistViewModel : PinglistViewModel
    {
    }

    public class PinglistListsViewModel
    {
        [Display(Name = "List ID")]
        public string ListId { get; set; }
        [Display(Name = "Secret key")] public string SecretKey { get; set; }
        public bool HasVerified { get; set; }
        public List<PinglistViewModel> OwnedLists { get; set; }
        public List<PinglistViewModel> OnLists { get; set; }
        public List<PinglistCategory> AvailableCategories { get; } = new List<PinglistCategory>();
    }

    public class EditPinglistViewModel : PinglistViewModel
    {
        public FormatModel Format { get; set; }
        [Display(Name = "Category")]
        public int? NewPinglistCategory { get; set; }

        public List<PinglistCategory> AvailableCategories { get; } = new List<PinglistCategory>();

        public class FormatModel
        {
            public string Prefix { get; set; }
            public string Postfix { get; set; }
            public string Separator { get; set; } = ", ";
        }
    }

    public class PinglistEntryViewModel
    {
        public FRUser FRUser { get; set; }
        public string Remarks { get; set; }
        public string EntryId { get; set; }
        public string SecretKey { get; set; }
    }

    public class PinglistEntriesViewModel
    {
        public List<PinglistEntryViewModel> PinglistEntries { get; set; }
        public bool IsPublic { get; set; }
        public int? CurrentUserId { get; set; }
        public string ListId { get; set; }
        public string SecretKey { get; set; }
        public int? CurrentFRUserId { get; set; }
    }

    [SmartRequired]
    public class AddEntryViewModel
    {
        [Required]
        [Display(Name = "List ID")]
        public string ListId { get; set; }
        [Display(Name = "User ID")]
        public int? UserId { get; set; }
        public string Username { get; set; }
        [IgnoreRequired]
        public string Remarks { get; set; }
        [IgnoreRequired]
        public string SecretKey { get; set; }
    }

    public class AddSelfEntryViewModel
    {
        [Required]
        public string ListId { get; set; }
        public string Remarks { get; set; }
        public string SecretKey { get; set; }
    }

    public class ManagePinglistEntryViewModel
    {
        public string ListId { get; set; }
        public string SecretKey { get; set; }
        public PinglistEntryViewModel EntryViewModel { get; set; }
    }

    public class RemoveEntryViewModel
    {
        public string ListId { get; set; }
        public string SecretKey { get; set; }
        public string EntryId { get; set; }
        public string EntrySecret { get; set; }
    }

    public class ImportPingsViewModel : PinglistViewModel
    {
        public string CSV { get; set; }
    }
}