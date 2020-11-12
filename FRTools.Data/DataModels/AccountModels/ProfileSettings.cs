namespace FRTools.Data.DataModels
{
    public class ProfileSettings
    {
        public int Id { get; set; }

        public string ProfileBio { get; set; }

        public bool PublicProfile { get; set; } = true;

        public bool ShowPreviewsOnProfile { get; set; } = true;
        public bool ShowSkinsOnProfile { get; set; } = true;
        public bool ShowPingListsOnProfile { get; set; } = true;
        public bool DefaultShowSkinsInBrowse { get; set; } = true;
        public bool DefaultSkinsArePublic { get; set; } = true;
        public bool ShowFRLinkStatus { get; set; } = true;
        public bool ShowAds { get; set; } = true;

        public virtual User User { get; set; }
    }
}
