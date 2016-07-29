using AutoMapper;
using ButiqueShops.Models;
using ButiqueShops.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ButiqueShops.Controllers
{
    public class ShopController : Controller
    {
        private ButiqueShopsEntities db;
        public ShopController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }
        // GET: Shop
        public async Task<ActionResult> AdminShopList()
        {
            var shops = await db.Shops.Include(s => s.AspNetUsers).ToListAsync(); ;
            var shopsViewModel = Mapper.Map<List<ShopViewModel>>(shops);
            return View("AdminShopList", shopsViewModel);
        }

        // GET: AddShop
        [Authorize]
        public ActionResult AddShop()
        {
            var users = db.AspNetUsers.ToList();
            var items = new List<SelectListItem>();
            foreach(var user in users)
                items.Add(new SelectListItem{ Text = user.UserName, Value = user.Id});
            ViewBag.OwnerId = items;
            return View();
        }

        // Post: AddShop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddShop(ShopViewModel shop)
        {
            shop.DateAdded = DateTime.Now;
            var shopModel = Mapper.Map<Shops>(shop);
            db.Shops.Add(shopModel);
            db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //[Authorize]
        //public ActionResult Delete(int id)
        //{

        //}
    }
}