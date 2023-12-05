using System.ComponentModel;

namespace FRTools.Core.Data.DataModels
{
    public enum SkinVisiblity
    {
        [Description("Skin is visible everywhere")]
        Visible,
        [Description("Hide skin from browse only")]
        HideFromBrowse,
        [Description("Hide skin everywhere")]
        HideEverywhere
    }

    public enum JobStatus
    {
        Running,
        Finished,
        Error,
        Cancelled,
        FinishedWithErrors,
        UserConfirmedDone
    }

    public enum DiscordChannelType
    {
        Text,
        Voice,
        Category,
        Other
    }
}