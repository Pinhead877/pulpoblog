using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ButiqueShops.Controllers
{
    public class ServicesController : Controller
    {
        private ButiqueShopsEntities db;

        public ServicesController()
        {
            db = new ButiqueShopsEntities();
        }

        public async Task<JsonResult> likeStore(int shopid)
        {
            var user = await db.AspNetUsers.Include(u=>u.UserLikeShop).FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            //var likes = user.UserLikeShop.Where(r => r.ShopId == shopid).ToList();
            var userliked = await db.UserLikeShop.FirstOrDefaultAsync(u => u.UserId == user.Id && u.ShopId == shopid);
            bool respond = false;
            if (userliked == null)
            {
                db.UserLikeShop.Add(new UserLikeShop { LikedOn = DateTime.Now, ShopId = shopid, IsActive = true, UserId = user.Id });
                respond = true;
            }
            else if (userliked.IsActive == true)
            {
                userliked.IsActive = false;
                respond = false;
                db.Entry(user).State = EntityState.Modified;
            }
            else if (userliked.IsActive == false)
            {
                userliked.IsActive = true;
                respond = true;
                db.Entry(user).State = EntityState.Modified;
            }
            db.SaveChangesAsync();
            return Json(respond, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> likeItem(int itemid)
        {
            var user = await db.AspNetUsers.Include(u => u.UserLikedItem).FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            var userliked = await db.UserLikedItem.FirstOrDefaultAsync(u => u.UserId == user.Id && u.ItemId==itemid);
            //var likes = user.UserLikedItem.Where(r => r.ItemId == itemid).ToList();
            bool respond = false;
            if (userliked == null)
            {
                db.UserLikedItem.Add(new UserLikedItem { LikedOn = DateTime.Now, ItemId = itemid, IsActive = true, UserId = user.Id });
                respond = true;
            }
            else if (userliked.IsActive == true)
            {
                userliked.IsActive = false;
                respond = false;
                db.Entry(user).State = EntityState.Modified;
            }
            else if (userliked.IsActive == false)
            {
                userliked.IsActive = true;
                respond = true;
                db.Entry(user).State = EntityState.Modified;
            }
            var result = await db.SaveChangesAsync();
            return Json(respond, JsonRequestBehavior.AllowGet);
        }
    }
}