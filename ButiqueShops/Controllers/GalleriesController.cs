﻿using ButiqueShops.Models;
using ButiqueShops.ViewModels;
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
            Shops shop = await db.Shops.Include(r => r.UserLikeShop).FirstOrDefaultAsync(s => s.Id == id);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
                var shopLiked = shop.UserLikeShop.FirstOrDefault(r => r.UserId == user.Id);
                ViewBag.likedShop = (shopLiked == null || shopLiked.IsActive == false) ? false : true;
                var visits = user.UserVisitedShop.Where(r => r.ShopId == id).ToList();
                DateTime lastVisit = (visits.Count == 0) ? DateTime.MinValue : visits.Select(r => r.Date).Max();
                if (DateTime.Now - lastVisit > new TimeSpan(0, 15, 0))
                {
                    user.UserVisitedShop.Add(new UserVisitedShop { ShopId = id, Date = DateTime.Now, UserId = user.Id });
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            return View(shop);
        }

        public async Task<ActionResult> Item(int id)
        {
            db = new ButiqueShopsEntities();
            Items item = await db.Items
                    .Include(f => f.UserLikedItem)
                    .Include(r => r.Sizes)
                    .Include(r => r.Shops)
                    .Include(s => s.Colors)
                    .Include(r => r.ItemTypes)
                    .FirstOrDefaultAsync(r => r.Id == id);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await db.AspNetUsers.Include(r => r.UserVistedItem).FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
                var itemLiked = item.UserLikedItem.FirstOrDefault(r => r.UserId == user.Id);
                ViewBag.likedItem = (itemLiked == null || itemLiked.IsActive == false) ? false : true;
                var visits = user.UserVistedItem.Where(r => r.ItemId == id).ToList();
                DateTime lastVisit = (visits.Count == 0) ? DateTime.MinValue : visits.Select(r => r.VisitedOn).Max();
                if (DateTime.Now - lastVisit > new TimeSpan(0, 15, 0))
                {
                    user.UserVistedItem.Add(new UserVistedItem { ItemId = id, VisitedOn = DateTime.Now, UserId = user.Id });
                    db.Entry(user).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }
            return View(item);
        }

        //public async Task<ActionResult> Items(ItemSpecification spec)
        //{
            //db = new ButiqueShopsEntities();
            //IQueryable<Items> query = db.Items;
            //if (spec.shopid != null)
            //{
            //    query = query.Where(i => i.ShopId==spec.shopid);
            //}
            //if (spec.sizeids != null)
            //{
            //    foreach (var size in spec.sizeids)
            //    {
            //        query = query.Where(i=>i.Sizes.Select(t=>t.Id).Contains(size));
            //    }
            //}
            //return View();
        //}

    }
}