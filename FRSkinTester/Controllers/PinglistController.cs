using FRTools.Data;
using FRTools.Data.DataModels;
using System;
using System.Collections.Generic;
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
            return View();
        }

        [Route("create", Name = "PinglistCreatePost")]
        [HttpPost]
        public ActionResult Create(object model)
        {
            return View();
        }

        [Route("list", Name = "Pinglist")]
        public ActionResult List()
        {
            return View();
        }

        [Route("list/{listId}", Name = "PinglistDirect")]
        public ActionResult List(string listId)
        {
            return View();
        }

        [Route("list/{listId}/addUser/{userName:alpha}")]
        public ActionResult AddUser(string listId, string username)
        {
            return AddUser(listId, username, 0);
        }

        [Route("list/{listId}/addUser/{userId:int}")]
        public ActionResult AddUser(string listId, int userId)
        {
            return AddUser(listId, "", userId);
        }

        private ActionResult AddUser(string listId, string username, int userId)
        {
            return View();
        }

        [Route("link", Name = "PinglistLink")]
        public ActionResult LinkExisting()
        {
            return View();
        }

        [Route("link", Name = "PinglistLinkPost")]
        [HttpPost]
        public ActionResult LinkExisting(string listId, string secretKey)
        {
            return View();
        }

        [Route("manage", Name = "PinglistManage")]
        public ActionResult ManageHome()
        {
            return View();
        }

        [Route("manage/{listId}/{secretKey}", Name = "PinglistManageList")]
        public ActionResult ManageList(string listId, string secretKey)
        {
            return View();
        }
        
        private Pinglist GetPinglist(string listId)
        {
            using (var ctx = new DataContext())
            {
                return ctx.Pinglists.FirstOrDefault(x => x.GeneratedId == listId);                      
            }
        }

        private List<PingListEntry> GetPinglistEnries(string listId)
        {
            using (var ctx = new DataContext())
            {
                return ctx.Pinglists.FirstOrDefault(x => x.GeneratedId == listId)?.Entries.ToList();
            }
        }
    }
}