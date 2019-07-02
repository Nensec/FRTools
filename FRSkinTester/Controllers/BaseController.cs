using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    public class BaseController : Controller
    {
        protected const string DressingRoomDummyUrl = "http://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        protected const string ScryerUrl = "http://flightrising.com/includes/scryer_getdragon.php?zord={0}";
        protected const string DragonProfileUrl = "http://flightrising.com/main.php?dragon={0}";
        protected const string DressingRoomApparalUrl = "http://www1.flightrising.com/dgen/dressing-room/dragon?did={0}&apparel={1}";


        private Random _random = new Random(Guid.NewGuid().GetHashCode());

        protected string GenerateId(int length = 7, IEnumerable<string> mustNotMatch = null)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id = "";
            for (int i = 0; i < length; i++)
                id += chars.Skip(_random.Next(chars.Length)).First();
            if (mustNotMatch?.Contains(id) == true)
            {
                return GenerateId(length, mustNotMatch);
            }
            return id;
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}