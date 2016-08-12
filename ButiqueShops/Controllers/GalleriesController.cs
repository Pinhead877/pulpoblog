using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ButiqueShops.Controllers
{
    public class GalleriesController : Controller
    {
        private ButiqueShopsEntities db;

        // GET: Galleries
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pants()
        {
            return View();
        }

        public ActionResult Shirts()
        {
            return View();
        }

        public ActionResult Dresses()
        {
            return View();
        }

        public async Task<ActionResult> Shop(int id)
        {
            db = new ButiqueShopsEntities();
            var shop = await db.Shops.Include(r=>r.UserLikeShop).FirstOrDefaultAsync(s => s.Id==id);
            var user = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            var shopLiked = shop.UserLikeShop.FirstOrDefault(r => r.UserId == user.Id);
            if (shopLiked==null || shopLiked.IsActive==false)
            {
                ViewBag.likedShop = false;
            }
            else
            {
                ViewBag.likedShop = true;
            }
            return View(shop);
        }
    }
}