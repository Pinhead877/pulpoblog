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
            var shop = await db.Shops.Include(r => r.UserLikeShop).FirstOrDefaultAsync(s => s.Id == id);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
                var shopLiked = shop.UserLikeShop.FirstOrDefault(r => r.UserId == user.Id);
                ViewBag.likedShop = (shopLiked == null || shopLiked.IsActive == false) ? false : true;
            }
            return View(shop);
        }

        public async Task<ActionResult> Item(int id)
        {
            db = new ButiqueShopsEntities();
            var item = await db.Items
                .Include(f => f.UserLikedItem)
                .Include(r => r.Sizes)
                .Include(r => r.Shops)
                .Include(s => s.Colors)
                .Include(r => r.ItemTypes)
                .FirstOrDefaultAsync(r => r.Id == id);
            var user = await db.AspNetUsers.Include(r=>r.UserVistedItem).FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            var itemLiked = item.UserLikedItem.FirstOrDefault(r => r.UserId == user.Id);
            ViewBag.likedItem = (itemLiked == null || itemLiked.IsActive == false) ? false : true;
            user.UserVistedItem.Add(new UserVistedItem { ItemId = id, VisitedOn = DateTime.Now });
            return View(item);
        }


    }
}