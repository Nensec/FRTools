using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Web.Infrastructure;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;
using FRTools.Web.Infrastructure.Managers;
using FRTools.Data.DataModels.PinglistModels;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("pinglist")]
    public class PinglistController : BaseController
    {
        [Route(Name = "PinglistInfo")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("create", Name = "PinglistCreate")]
        public ActionResult Create()
        {
            return View(new CreatePinglistViewModel());
        }

        [Route("create", Name = "PinglistCreatePost")]
        [HttpPost]
        public ActionResult Create(CreatePinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var pinglist = ctx.Pinglists.Add(new Pinglist());
                pinglist.GeneratedId = GenerateId(5, ctx.Pinglists.Select(x => x.GeneratedId).ToList());
                pinglist.SecretKey = GenerateId();
                pinglist.Name = model.Name;
                pinglist.IsPublic = model.IsPublic;

                if (Request.IsAuthenticated)
                {
                    int userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                    pinglist.Creator = ctx.Users.Find(userId);
                }
                else
                    TempData["NewList"] = (pinglist.GeneratedId, pinglist.SecretKey);

                ctx.SaveChanges();

                return RedirectToRoute("PinglistDirect", new { listId = pinglist.GeneratedId, secretKey = pinglist.SecretKey });
            }
        }

        [Route("delete", Name = "PinglistDelete")]
        public ActionResult Delete(PinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);
                if (IsOwner(list, model.SecretKey))
                {
                    ctx.PingListEntries.RemoveRange(list.Entries.ToList());
                    ctx.Pinglists.Remove(list);
                    ctx.SaveChanges();

                    TempData["Success"] = "The pinglist has been succesfully removed.";
                }
                else
                {
                    TempData["Error"] = "Only the owner can manage a pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistDirect", new { listId = model.ListId });
                }
            }

            return RedirectToRoute("Pinglist");
        }

        [Route("list", Name = "Pinglist")]
        public ActionResult List()
        {
            var model = new PinglistListsViewModel();
            using (var ctx = new DataContext())
            {
                if (Request.IsAuthenticated)
                {
                    var currentUser = ctx.Users.Find(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>());
                    var ownedLists = ctx.Pinglists.Where(x => x.Creator.Id == currentUser.Id);
                    model.OwnedLists = ownedLists.Select(x => new PinglistViewModel { Name = x.Name, ListId = x.GeneratedId, IsPublic = x.IsPublic, PinglistCategory = x.PinglistCategory }).ToList();
                    model.AvailableCategories.AddRange(ctx.PinglistCategories.Where(x => x.Owner.Id == currentUser.Id));

                    if (currentUser.FRUser != null)
                    {
                        var onLists = ctx.Pinglists.Where(x => x.Entries.Any(e => e.FRUser.Id == currentUser.FRUser.Id));

                        model.OnLists = onLists.Select(x => new PinglistViewModel { Name = x.Name, ListId = x.GeneratedId, IsPublic = x.IsPublic, Owner = x.Creator, PinglistCategory = x.PinglistCategory }).ToList();
                        model.HasVerified = true;
                    }
                    else
                        model.HasVerified = false;
                }
            }

            return View(model);
        }

        [Route("list/{listId}", Name = "PinglistDirect")]
        public ActionResult List(string listId, string secretKey = null)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(listId, true, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistInfo");

                var model = new PinglistViewModel
                {
                    Name = list.Name,
                    Owner = list.Creator,
                    ListId = list.GeneratedId,
                    IsPublic = list.IsPublic
                };

                if (Request.IsAuthenticated)
                    model.CurrentFRUser = ctx.Users.Find(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>()).FRUser;

                if (!HasAccess(list, secretKey))
                {
                    if (list.Entries.Any(x => x.FRUser.FRId == model.CurrentFRUser?.FRId))
                    {
                        TempData["Info"] = "This pinglist is private, however since you are on it you can manage your entry.";
                        var entry = list.Entries.FirstOrDefault(x => x.FRUser.Id == model.CurrentFRUser.Id);
                        model.EntriesViewModel = new PinglistEntriesViewModel
                        {
                            IsPublic = model.IsPublic,
                            CurrentUserId = entry.FRUser.User.Id,
                            CurrentFRUserId = entry.FRUser.FRId,
                            ListId = list.GeneratedId,
                            PinglistEntries = new[] { new PinglistEntryViewModel { EntryId = entry.GeneratedId, SecretKey = entry.SecretKey, FRUser = model.CurrentFRUser } }.ToList()
                        };
                        return View("PrivateViewList", model);
                    }
                    else
                    {
                        TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                        return RedirectToRoute("Pinglist");
                    }
                }
                else
                    model.EntriesViewModel = new PinglistEntriesViewModel
                    {
                        ListId = list.GeneratedId,
                        PinglistEntries = list.Entries.Select(x => new PinglistEntryViewModel { EntryId = x.GeneratedId, SecretKey = x.SecretKey, FRUser = x.FRUser, Remarks = x.Remarks }).ToList(),
                        IsPublic = model.IsPublic,
                        CurrentUserId = model.CurrentFRUser?.User.Id,
                        CurrentFRUserId = model.CurrentFRUser?.FRId
                    };
                if (IsOwner(list, secretKey))
                {
                    var ownerModel = new EditPinglistViewModel { Name = model.Name, Owner = model.Owner, ListId = model.ListId, CurrentFRUser = model.CurrentFRUser, EntriesViewModel = model.EntriesViewModel, IsPublic = list.IsPublic };
                    ownerModel.EntriesViewModel.SecretKey = ownerModel.SecretKey = list.SecretKey;
                    ownerModel.Format = list.Format == null ? new EditPinglistViewModel.FormatModel() : JsonConvert.DeserializeObject<EditPinglistViewModel.FormatModel>(list.Format);
                    ownerModel.CopyPinglist = $"{ownerModel.Format.Prefix}{string.Join(ownerModel.Format.Separator, ownerModel.EntriesViewModel.PinglistEntries.Select(x => $"@{x.FRUser.Username}"))}{ownerModel.Format.Postfix}";
                    ownerModel.AvailableCategories.Add(new PinglistCategory { Id = -1 });
                    ownerModel.AvailableCategories.AddRange(ctx.PinglistCategories.Where(x => x.Owner.Id == ownerModel.Owner.Id).ToList());
                    ownerModel.PinglistCategory = list.PinglistCategory;
                    var activeJobs = JobManager.GetActiveJobs(list.Id.ToString());
                    foreach (var job in activeJobs)
                    {
                        if (!string.IsNullOrEmpty(TempData["Info"] as string))
                            TempData["Info"] += "<br/>";
                        TempData["Info"] += job.Description;
                    }
                    return View("OwnerViewList", ownerModel);
                }

                return View("PublicViewList", model);
            }
        }

        [Route("list/{listId}/addEntry", Name = "PinglistAddEntryPost")]
        [HttpPost]
        public async Task<ActionResult> AddEntry(AddEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistInfo");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("Pinglist");
                }

                var entry = await AddEntryToList(list, model.Username, model.UserId, model.Remarks, ctx);
                if (entry == null)
                {
                    TempData["Error"] = $"Could not find user '{(model.UserId?.ToString() ?? model.Username)}' on Flight Rising. Verify the name or id is correct.";
                    return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
                }
                ctx.SaveChanges();

                if (!IsOwner(list, model.SecretKey))
                    TempData["NewEntry"] = (entry.GeneratedId, entry.SecretKey, entry.FRUser.Username);

                return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
            }
        }

        [Route("list/{listId}/addSelf", Name = "PinglistAddSelfPost")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddSelf(AddSelfEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistLink");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistDirect", new { listId = model.ListId });
                }

                var currentUser = ctx.Users.Find(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>());

                if (currentUser.FRUser == null)
                {
                    TempData["Error"] = $"To use this feature you need to link your Flight Rising account, you can do so <a href=\"{Url.RouteUrl("VerifyFR")}\">here</a>.";
                    return RedirectToRoute("PinglistDirect");
                }

                await AddEntryToList(list, null, currentUser.FRUser.FRId, model.Remarks, ctx);
                ctx.SaveChanges();
            }
            return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
        }

        [Route("list/{listId}/removeEntry/{entryId}/{entrySecret}")]
        public ActionResult RemoveEntry(RemoveEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistLink");

                var entry = list.Entries.FirstOrDefault(x => x.GeneratedId == model.EntryId);
                var currentUser = ctx.Users.Find(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>());

                if (entry == null || (entry.SecretKey != model.EntrySecret && (currentUser.FRUser == null || entry.FRUser.Id != currentUser.FRUser.Id)))
                {
                    TempData["Error"] = $"Could not find entry id '{model.EntryId}' in list '{list.Name}'.";
                    return RedirectToRoute("PinglistDirect", new { model.ListId });
                }

                list.Entries.Remove(entry);
                TempData["Success"] = $"The entry for user '{entry.FRUser.Username}' has been removed.";
                ctx.SaveChanges();

                return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
            }
        }

        [Route("link", Name = "PinglistLink")]
        [Authorize]
        public ActionResult LinkExisting()
        {
            return View(new PinglistViewModel());
        }

        [Route("link", Name = "PinglistLinkPost")]
        [HttpPost]
        [Authorize]
        public ActionResult LinkExisting(PinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistLink");

                if (list.Creator != null)
                {
                    TempData["Error"] = "This list is already linked to an account";
                    return RedirectToRoute("PinglistLink");
                }

                if (list.SecretKey != model.SecretKey)
                {
                    TempData["Error"] = "Secret key does not match";
                    return RedirectToRoute("PinglistLink");
                }

                int userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

                list.Creator = ctx.Users.Find(userId);
                TempData["Success"] = "Pinglist has been succesfully linked to your account";

                ctx.SaveChanges();
            }
            return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
        }

        [Route("entry/manage", Name = "PinglistEntryManage")]
        [HttpPost]
        public ActionResult ManageEntry(ManagePinglistEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistLink");

                var entry = list.Entries.FirstOrDefault(x => x.GeneratedId == model.EntryViewModel.EntryId);

                if (entry == null || entry.SecretKey != model.EntryViewModel.SecretKey)
                {
                    TempData["Error"] = $"Could not find entry id '{model.EntryViewModel.EntryId}' in list '{list.Name}'";
                    return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
                }

                entry.Remarks = model.EntryViewModel.Remarks;
                ctx.SaveChanges();

                TempData["Info"] = $"Entry '{entry.GeneratedId} ' updated.";
                return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
            }
        }

        [Route("manage/{listId}/{secretKey}", Name = "PinglistManageListPost")]
        [HttpPost]
        public ActionResult ManageList(EditPinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("Pinglist");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("Pinglist");
                }

                if (!IsOwner(list, model.SecretKey))
                {
                    TempData["Error"] = "Only the owner can manage a pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistDirect", new { model.ListId });
                }

                list.Name = model.Name;
                list.IsPublic = model.IsPublic;
                list.PinglistCategory = Request.IsAuthenticated && model.NewPinglistCategory.HasValue && list.Creator != null ? ctx.PinglistCategories.FirstOrDefault(x => x.Owner.Id == list.Creator.Id && x.Id == model.NewPinglistCategory.Value) : null;
                list.Format = JsonConvert.SerializeObject(model.Format);

                ctx.SaveChanges();

                return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
            }
        }

        [Route("manage/{listId}/{secretKey}/import", Name = "PinglistImportCSV")]
        [HttpPost]
        public ActionResult ImportPinglist(ImportPingsViewModel model)
        {
            int listId;
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, false, ctx);

                if (list == null)
                    return RedirectToRoute("Pinglist");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("Pinglist");
                }

                listId = list.Id;
            }

            if (!string.IsNullOrWhiteSpace(model.CSV))
            {
                var job = Task.Run(async () =>
                {
                    using (var ctx = new DataContext())
                    {
                        var list = GetPinglist(model.ListId, false, ctx);

                        var usernames = model.CSV.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var username in usernames)
                        {
                            if (username.All(char.IsDigit))
                                await AddEntryToList(list, null, int.Parse(username), null, ctx);
                            else
                                await AddEntryToList(list, username, null, null, ctx);
                        }
                        ctx.SaveChanges();
                    }
                });
                var jobResult = JobManager.StartNewJob(job, listId.ToString(), $"Importing pinglist for pinglist '{model.ListId}'");
                TempData["Info"] = $"Your pinglist is being imported in the background, depending on the size this can take a while. You started this job at '{jobResult.StartTime}'.";
            }

            return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
        }

        [Route("manage/{listId}/{secretKey}/updateusers", Name = "PinglistBatchUpdateUsers")]
        public ActionResult UpdatePinglistUsers(PinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, true, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistDirect");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistDirect", new { model.ListId });
                }

                if (!IsOwner(list, model.SecretKey))
                {
                    TempData["Error"] = "Only the owner can manage a pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistDirect", new { model.ListId });
                }

                var job = Task.Run(async () =>
                {
                    foreach (var entry in list.Entries.Select(x => x.FRUser.FRId).ToList())
                        await FRHelpers.GetOrUpdateFRUser(entry);
                });
                var jobResult = JobManager.StartNewJob(job, list.Id.ToString(), $"Updating entries for pinglist '{model.ListId}'");
                TempData["Info"] = $"The entries on your pinglist are being updated in the background, depending on the size this can take a while. You started this job at '{jobResult.StartTime}'.";
            }

            return RedirectToRoute("PinglistDirect", new { model.ListId, model.SecretKey });
        }

        [Route("manage/category/create", Name = "PinglistCategoryAdd")]
        [HttpPost]
        [Authorize]
        public ActionResult AddCategory(string name)
        {
            using (var ctx = new DataContext())
            {
                var user = ctx.Users.Find(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>());
                var cat = ctx.PinglistCategories.Add(new PinglistCategory { Name = name, Owner = user });
                ctx.SaveChanges();
                return Json(new { Result = 1, cat.Id, cat.Name });
            }
        }

        [Route("manage/category/edit", Name = "PinglistCategoryEdit")]
        [HttpPost]
        [Authorize]
        public ActionResult EditCategory(int id, string name)
        {
            using (var ctx = new DataContext())
            {
                var cat = ctx.PinglistCategories.Include(x => x.Owner).FirstOrDefault(x => x.Id == id);
                if (cat.Owner.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                {
                    cat.Name = name;
                    ctx.SaveChanges();
                    return Json(new { cat.Id, cat.Name });
                }
                else
                    return Json(new { Result = -1 });
            }
        }

        [Route("manage/category/delete", Name = "PinglistCategoryDelete")]
        [HttpPost]
        [Authorize]
        public ActionResult DeleteCategory(int id)
        {
            using (var ctx = new DataContext())
            {
                var cat = ctx.PinglistCategories.Include(x => x.Owner).Include(x =>x.Pinglists).FirstOrDefault(x => x.Id == id);
                if (cat.Owner.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                {
                    foreach(var pinglist in cat.Pinglists)                    
                        pinglist.PinglistCategory = null;
                    ctx.PinglistCategories.Remove(cat);
                    ctx.SaveChanges();
                }
                return Json(new { Result = -1 });
            }
        }

        private bool HasAccess(Pinglist list, string secretKey)
        {
            if (list.IsPublic || (list.SecretKey == secretKey && list.Creator == null) || (list.Creator != null && list.Creator.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>()))
                return true;

            return false;
        }

        private bool IsOwner(Pinglist list, string secretKey)
        {
            if (list.Creator == null && list.SecretKey == secretKey)
                return true;
            if (list.Creator != null && Request.IsAuthenticated && list.Creator.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                return true;

            return false;
        }

        private async Task<PingListEntry> AddEntryToList(Pinglist list, string username, int? userId, string remarks, DataContext ctx)
        {
            username = username?.Trim();
            var frUser = await (username != null ? FRHelpers.GetOrUpdateFRUser(username, ctx) : FRHelpers.GetOrUpdateFRUser(userId.Value, ctx));
            if (frUser == null)
            {
                if (!string.IsNullOrEmpty(TempData["Error"] as string))
                    TempData["Error"] += "<br/>";
                TempData["Error"] += $"Could not validate the existence of user '{username ?? userId.ToString()}.'";
                return null;
            }

            if (ctx.Pinglists.Find(list.Id).Entries.Any(x => x.FRUser.Id == frUser.Id))
            {
                if (!string.IsNullOrEmpty(TempData["Info"] as string))
                    TempData["Info"] += "<br/>";
                TempData["Info"] += $"User '{frUser.Username}' is already on the pinglist.";
                return list.Entries.FirstOrDefault(x => x.FRUser.Id == frUser.Id);
            }

            var entry = new PingListEntry
            {
                FRUser = frUser,
                GeneratedId = GenerateId(5, list.Entries.Select(x => x.GeneratedId).ToList()),
                SecretKey = GenerateId(),
                Remarks = remarks
            };
            list.Entries.Add(entry);

            if (!string.IsNullOrEmpty(TempData["Success"] as string))
                TempData["Success"] += "<br/>";
            TempData["Success"] += $"User '{frUser.Username}' has been added to the pinglist.";

            return entry;
        }

        private Pinglist GetPinglist(string listId, bool includeUsers, DataContext ctx = null)
        {
            Pinglist getList()
            {
                IQueryable<Pinglist> query = ctx.Pinglists;
                if (includeUsers)
                    query = query.Include(x => x.Entries.Select(e => e.FRUser));
                var list = query.FirstOrDefault(x => x.GeneratedId == listId);
                if (list == null)
                    TempData["Error"] = $"There is no pinglist with the ID '{listId}'.";

                return list;
            }

            if (ctx == null)
                using (ctx = new DataContext())
                    return getList();
            else
                return getList();
        }
    }
}