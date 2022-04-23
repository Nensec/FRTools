using System;

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
        public string DefaultAdvancedCoverageBackgroundColor { get; set; } = "#FFFFFF";
        public string DefaultAdvancedCoverageOverlayColor { get; set; } = "#FF0000";
        public int DefaultAdvancedCoverageDummyOpacity { get; set; } = 40;
        public int DefaultAdvancedCoverageSkinOpacity { get; set; } = 40;
        public int DefaultAdvancedCoveragePercentagePrecision { get; set; } = 2;
        public bool DefaultAdvancedCoverageUseDressingRoomBase { get; set; } = true;
        public string DefaultAdvancedCoverageScry { get; set; } = "";


        public virtual User User { get; set; }

        public object this[string key]
        {
            get => GetType().GetProperty(key)?.GetValue(this);
            set => GetType().GetProperty(key)?.SetValue(this, Convert.ChangeType(value, GetType().GetProperty(key).PropertyType));
        }
    }
}
