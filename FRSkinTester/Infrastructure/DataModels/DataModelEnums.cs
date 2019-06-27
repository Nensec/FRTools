using System.ComponentModel;

namespace FRSkinTester.Infrastructure.DataModels
{
    public enum Privacy
    {
        [Description("Show all")]
        ShowAll,
        [Description("Hide previews")]
        HidePreviews,
        [Description("Hide skins")]
        HideSkins,
        [Description("Make profile private")]
        HideAll
    }

    public enum SkinVisiblity
    {
        [Description("Skin is visible everywhere")]
        Visible,
        [Description("Hide skin from browse only")]
        HideFromBrowse,
        [Description("Hide skin everywhere")]
        HideEverywhere
    }
}