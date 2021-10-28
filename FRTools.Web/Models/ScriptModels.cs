using System;
using System.Collections.Generic;
using System.Linq;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;

namespace FRTools.Web.Models
{
    public abstract class BaseScriptModel
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string IconBase64 { get; }

        private string _sanitizedName = null;
        public string SanitizedName => _sanitizedName ?? (_sanitizedName = Name.ToLower().Replace(" ", "_"));

        public virtual Action Initalize { get; }
    }

    public class AvatarImageScriptModel : BaseScriptModel
    {
        public override string Name => "Avatar image";
        public override string Description => "Generates the avatar version of a dragon image.";
        public override string IconBase64 => null;
    }

    public class ColiImageScriptModel : BaseScriptModel
    {
        public override string Name => "Coli image";
        public override string Description => "Generates the coliseum version of a dragon image.";
        public override string IconBase64 => null;
    }

    public class SmallImageScriptModel : BaseScriptModel
    {
        public override string Name => "Small image";
        public override string Description => "Generates the smaller version of a dragon image.";
        public override string IconBase64 => null;
    }

    public class SkinCoverageScriptModel : BaseScriptModel
    {
        public override string Name => "Skin coverage";
        public override string Description => "Calculates and shows you the coverage of your skin, and what's outside.";
        public override string IconBase64 => null;
    }

    public class ColorWheelScriptModel : BaseScriptModel
    {
        public override string Name => "Color wheel";
        public override string Description => "A way to easily visualze the different colors and their HEX codes.";
        public override string IconBase64 => null;
    }

    public class ScenePreviewerScriptModel : BaseScriptModel
    {
        public override string Name => "Scene previewer";
        public override string Description => "Preview scenes for your dragon, without needing to purchase the scene first.";
        public override string IconBase64 => null;

        public Dictionary<int, string> Scenes { get; private set; }

        public Dictionary<int, string> Familiars { get; private set; }

        public override Action Initalize => () =>
        {
            using(var ctx = new DataContext())
            {
                Scenes = ctx.FRItems
                .Where(x => x.ItemCategory == FRItemCategory.Trinket && x.ItemType == "Scene")
                .ToDictionary(x => x.FRId, x => x.Name.Replace("Scene:","").Trim());

                Familiars = ctx.FRItems
                .Where(x => x.ItemCategory == FRItemCategory.Familiar)
                .ToDictionary(x => x.FRId, x => x.Name);
            }
        };
    }
}