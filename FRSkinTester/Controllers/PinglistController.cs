﻿using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Web.Infrastructure;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

                ctx.SaveChanges();
                return RedirectToRoute("PinglistDirect", new { listId = pinglist.GeneratedId, secretKey = pinglist.SecretKey });
            }
        }

        [Route("list", Name = "Pinglist")]
        public ActionResult List()
        {
            return View(new PinglistViewModel());
        }

        [Route("list/{listId}", Name = "PinglistDirect")]
        public ActionResult List(string listId, string secretKey = null)
        {
            var list = GetPinglist(listId);

            if (list == null)
                return RedirectToRoute("PinglistInfo");

            if (!HasAccess(list, secretKey))
            {
                TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                return RedirectToRoute("PinglistManage");
            }

            var model = new PinglistViewModel
            {
                Name = list.Name,
                Entries = list.Entries.Select(x => new PinglistEntryViewModel { FRUser = x.FRUser, Remarks = x.Remarks }).ToList()
            };
            return View(model);
        }

        [Route("list/{listId}/addEntry", Name = "PinglistAddEntryPost")]
        [HttpPost]
        public ActionResult AddEntry(AddEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistInfo");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistManage");
                }

                AddEntryToList(list, model.Username, model.UserId, model.Remarks, ctx);

                ctx.SaveChanges();

                return RedirectToRoute("PinglistDirect", new { listId = model.ListId });
            }
        }

        [Route("list/{listId}/addSelf")]
        [Authorize]
        [HttpPost]
        public ActionResult AddSelf(AddSelfEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, ctx);

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
                    TempData["Error"] = $"To use this feature you need to link your Flight Rising account, you can do so <a href=\"{Url.RouteUrl("VerifyFR")}\">here</a>";
                    return RedirectToRoute("PinglistDirect");
                }

                AddEntryToList(list, null, currentUser.FRUser.FRId, null, ctx);
            }
            return RedirectToRoute("PinglistDirect", new { listId = model.ListId });
        }

        [Route("list/{listId}/removeEntry/{entryId}/{entrySecret}")]
        public ActionResult RemoveEntry(string listId, string entryId, string entrySecret)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(listId, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistLink");

                var entry = list.Entries.FirstOrDefault(x => x.GeneratedId == entryId);
                
                if (entry == null || entry.SecretKey != entrySecret)
                {
                    TempData["Error"] = $"Could not find entry id '{entryId}' in list '{list.Name}'";
                    return RedirectToRoute("PinglistDirect", new { listId });
                }

                list.Entries.Remove(entry);
                TempData["Success"] = $"The entry for user '{entry.FRUser.Username}' has been removed.";
                ctx.SaveChanges();

                return RedirectToRoute("PinglistDirect", new { listId });
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
                var list = GetPinglist(model.ListId, ctx);

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
                ctx.SaveChanges();
            }
            return RedirectToRoute("PinglistManageList", new { listId = model.ListId, secretKey = model.SecretKey });
        }

        [Route("entry/manage")]
        [HttpPost]
        public ActionResult ManageEntry()
        {
            return View(new ManagePinglistEntryViewModel());
        }

        [Route("entry/manage")]
        [HttpPost]
        public ActionResult ManageEntry(ManagePinglistEntryViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistLink");

                var entry = list.Entries.FirstOrDefault(x => x.GeneratedId == model.EntryViewModel.EntryId);

                if (entry == null || entry.SecretKey != model.EntryViewModel.SecretKey)
                {
                    TempData["Error"] = $"Could not find entry id '{model.EntryViewModel.EntryId}' in list '{list.Name}'";
                    return RedirectToRoute("PinglistDirect", new { model.ListId });
                }

                entry.Remarks = model.EntryViewModel.Remarks;
                ctx.SaveChanges();

                return View();
            }
        }

        [Route("manage", Name = "PinglistManage")]
        public ActionResult ManageHome()
        {
            return View(new ManagePinglistsViewModel());
        }

        [Route("manage/{listId}/{secretKey}", Name = "PinglistManageList")]
        public ActionResult ManageList(EditPinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistManage");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistManage");
                }

                list.Name = model.Name;
                list.IsPublic = model.IsPublic;

                ctx.SaveChanges();

                return View();
            }
        }

        [Route("manage/{listId}/{secretKey}/import/{csv}")]
        public ActionResult ImportPinglist(ImportPingsViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistManage");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistManage");
                }

                var usernames = model.CSV.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var username in usernames)
                {
                    if (username.All(char.IsDigit))
                        AddEntryToList(list, null, int.Parse(username), null, ctx);
                    else
                        AddEntryToList(list, username, null, null, ctx);
                }
                ctx.SaveChanges();

                return RedirectToRoute("PinglistManageList");
            }
        }

        [Route("myentries")]
        [Authorize]
        public ActionResult MyEntries()
        {
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.Users.Find(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>());

                if (currentUser.FRUser == null)
                {
                    TempData["Error"] = $"To use this feature you need to link your Flight Rising account, you can do so <a href=\"{Url.RouteUrl("VerifyFR")}\">here</a>";
                    return RedirectToRoute("PinglistInfo");
                }

                var lists = ctx.Pinglists.Where(x => x.Entries.Any(e => e.FRUser.Id == currentUser.FRUser.Id));
                return View(new PinglistListsViewModel { Lists = lists.Select(x => new PinglistViewModel { Name = x.Name, ListId = x.GeneratedId, IsPublic = x.IsPublic }).ToList() });
            }
        }

        [Route("manage/{listId}/updateusers")]
        public ActionResult UpdatePinglistUsers(PinglistViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var list = GetPinglist(model.ListId, ctx);

                if (list == null)
                    return RedirectToRoute("PinglistDirect");

                if (!HasAccess(list, model.SecretKey))
                {
                    TempData["Error"] = "You do not have access to this pinglist. Make sure you are logged in or provide the correct secret key.";
                    return RedirectToRoute("PinglistDirect");
                }

                foreach (var entry in list.Entries.Select(x => x.FRUser.FRId).ToList())
                {
                    FRHelpers.GetOrUpdateFRUser(entry);
                }
            }

            return RedirectToRoute("PinglistDirect");
        }

        private bool HasAccess(Pinglist list, string secretKey)
        {
            if (list.IsPublic || (list.SecretKey == secretKey && list.Creator == null) || list.Creator.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                return true;

            return false;
        }

        private void AddEntryToList(Pinglist list, string username, int? userId, string remarks, DataContext ctx)
        {
            username = username?.Trim();
            var frUser = username != null ? FRHelpers.GetOrUpdateFRUser(username, ctx) : FRHelpers.GetOrUpdateFRUser(userId.Value, ctx);
            if (frUser == null)
            {
                if (!string.IsNullOrEmpty(TempData["Error"] as string))
                    TempData["Error"] += "<br/>";
                TempData["Error"] += $"Could not validate the existance of user '{username ?? userId.ToString()}'";
                return;
            }

            if (list.Entries.Any(x => x.FRUser.Id == frUser.Id))
            {
                if (!string.IsNullOrEmpty(TempData["Success"] as string))
                    TempData["Success"] += "<br/>";
                TempData["Success"] += $"User '{frUser.Username}' is already on the pinglist";
                return;
            }

            list.Entries.Add(new PingListEntry
            {
                FRUser = frUser,
                GeneratedId = GenerateId(5, list.Entries.Select(x => x.GeneratedId).ToList()),
                SecretKey = GenerateId(),
                Remarks = remarks
            });
        }

        private Pinglist GetPinglist(string listId, DataContext ctx = null)
        {
            Pinglist getList()
            {
                var list = ctx.Pinglists.FirstOrDefault(x => x.GeneratedId == listId);
                if (list == null)
                    TempData["Error"] = $"There is no pinglist with the ID '{listId}'";

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