using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class PreviewController : BaseController
    {
        [Route("Preview/{skinId}", Name = "Preview")]
        [Route("Home/Preview")]
        public async Task<ActionResult> Preview(PreviewModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }
                if (skin.Coverage == null)
                    await UpdateCoverage(skin, ctx);
                return View(new PreviewModelPost
                {
                    Title = skin.Title,
                    Description = skin.Description,
                    SkinId = model.SkinId,
                    PreviewUrl = await GenerateOrFetchPreview(model.SkinId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null),
                    Coverage = skin.Coverage
                });
            }
        }

        [HttpPost]
        [Route("Preview/{skinId}", Name = "PreviewPost")]
        public async Task<ActionResult> Preview(PreviewModelPost model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Preview", new { model.SkinId });

            string dwagonUrl = null;
            using (var client = new WebClient())
            {
                var htmlPage = await client.DownloadStringTaskAsync(new Uri(string.Format(ScryerUrl, model.DragonId)));
                dwagonUrl = ScrapeImageUrl(htmlPage);
            }

            if (dwagonUrl.StartsWith(".."))
            {
                TempData["Error"] = $"<b>{model.DragonId}</b> appears to be an invalid dragon id";
                return RedirectToAction("Preview", new { model.SkinId });
            }

            var dragon = ParseUrlForDragon(dwagonUrl);

            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }

                if (skin.DragonType != (int)dragon.DragonType)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(DragonType)skin.DragonType} {(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.DragonType} {dragon.Gender}</b>";
                    return RedirectToAction("Preview", new { model.SkinId });
                }

                if (skin.GenderType != (int)dragon.Gender)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.Gender}</b>";
                    return RedirectToAction("Preview", new { model.SkinId });
                }

                var url = await GenerateOrFetchPreview(model.SkinId, model.DragonId.ToString(), dwagonUrl, dragon);
                if (url == null)
                    return RedirectToAction("Preview", new { model.SkinId });

                skin.Previews.Add(new Preview
                {
                    DragonId = model.DragonId,
                    PreviewImage = url,
                    DragonData = dragon.ToString()
                });

                await ctx.SaveChangesAsync();

                return View("PreviewResult", new PreviewModelPostViewModel
                {
                    SkinId = model.SkinId,
                    ImageResultUrl = url,
                    Dragon = dragon
                });
            }
        }

        [Route("Preview/{skinId}/Scry", Name = "PreviewScryer")]
        [Route("Home/PreviewScryer")]
        public async Task<ActionResult> PreviewScryer(PreviewModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }
                if (skin.Coverage == null)
                    await UpdateCoverage(skin, ctx);

                return View(new PreviewScryerModelPost
                {
                    Title = skin.Title,
                    Description = skin.Description,
                    SkinId = model.SkinId,
                    PreviewUrl = await GenerateOrFetchPreview(model.SkinId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null),
                    Coverage = skin.Coverage
                });
            }
        }

        [HttpPost]
        [Route("Preview/{skinId}/Scry", Name = "PreviewScryerPost")]
        public async Task<ActionResult> PreviewScryer(PreviewScryerModelPost model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("PreviewScryer", new { model.SkinId });

            var dragon = ParseUrlForDragon(model.ScryerUrl);

            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }

                if (skin.DragonType != (int)dragon.DragonType)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(DragonType)skin.DragonType} {(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.DragonType} {dragon.Gender}</b>";
                    return RedirectToAction("PreviewScryer", new { model.SkinId });
                }

                if (skin.GenderType != (int)dragon.Gender)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.Gender}</b>";
                    return RedirectToAction("PreviewScryer", new { model.SkinId });
                }


                var url = await GenerateOrFetchPreview(model.SkinId, null, model.ScryerUrl, dragon);
                if (url == null)
                    return RedirectToAction("PreviewScryer", new { model.SkinId });

                skin.Previews.Add(new Preview
                {
                    ScryerUrl = model.ScryerUrl,
                    PreviewImage = url,
                    DragonData = dragon.ToString()
                });

                await ctx.SaveChangesAsync();

                return View("PreviewResult", new PreviewModelPostViewModel
                {
                    SkinId = model.SkinId,
                    ImageResultUrl = url,
                    Dragon = dragon
                });
            }
        }
    }
}