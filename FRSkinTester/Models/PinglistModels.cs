using FRTools.Data.DataModels;
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
        public string ListId { get; set; }
        public string SecretKey { get; set; }
        public PinglistEntriesViewModel EntriesViewModel { get; set; }
        public FRUser CurrentFRUser { get; set; }
        public User Owner { get; set; }
    }

    public class CreatePinglistViewModel : PinglistViewModel
    {
    }

    public class PinglistListsViewModel
    {
        public List<PinglistViewModel> Lists { get; set; }
    }

    public class EditPinglistViewModel : PinglistViewModel
    {
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
        public bool IsPrivate { get; set; }
        public bool IsOwner { get; set; }
        public int? CurrentUserId { get; set; }
        public string ListId { get; set; }
    }

    [SmartRequired]
    public class AddEntryViewModel
    {
        [Required]
        public string ListId { get; set; }
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

    public class ManagePinglistsViewModel
    {
        public EditPinglistViewModel EditViewModel { get; set; }
        public PinglistListsViewModel ListsViewModel { get; set; }
    }

    public class ManagePinglistEntryViewModel
    {
        public string ListId { get; set; }
        public PinglistEntryViewModel EntryViewModel { get; set; }
    }

    public class ImportPingsViewModel : PinglistViewModel
    {
        public string CSV { get; set; }
    }
}