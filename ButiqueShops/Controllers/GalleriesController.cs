using AutoMapper;
using ButiqueShops.Extensions;
using ButiqueShops.Models;
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
    /// <summary>
    /// Controller for all the client side pages
    /// </summary>
    public class GalleriesController : Controller
    {
        private ButiqueShopsEntities db;
        /// <summary>
        /// Contructor
        /// </summary>
        public GalleriesController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }
        /// <summary>
        /// opens the order page 
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult OrderPage(int itemid)
        {
            return View();
        }
        /// <summary>
        /// Show details about the shop to the client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Shop(int id)
        {
            Shops shop = null;
            shop = await db.Shops.Include(r => r.UserLikeShop).FirstOrDefaultAsync(s => s.Id == id);
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
                if (shop.OwnerId == user.Id) ViewBag.IsShopOwner = true;
            }
            return View(shop);
        }

        /// <summary>
        /// Show details about the item to the client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Item(int id)
        {
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
                if(item.Shops.OwnerId == user.Id) ViewBag.IsShopOwner = true;
            }
            return View(item);
        }

        /// <summary>
        /// Gallery view of the shops
        /// </summary>
        /// <param name="spec">filter the list</param>
        /// <returns></returns>
        public async Task<ActionResult> ShopsList(ShopSpecification spec)
        {
            IQueryable<Shops> query = db.Shops;
            if (!string.IsNullOrEmpty(spec.ownerid))
            {
                query = query.Where(s => s.OwnerId == spec.ownerid);
            }
            if (!string.IsNullOrEmpty(spec.userliked))
            {
                query = query.Include(r => r.UserLikeShop).Where(i => i.UserLikeShop.Where(r => r.IsActive).Select(u => u.UserId).Contains(spec.userliked));
            }
            if (!string.IsNullOrEmpty(spec.uservisited))
            {
                query = query.Include(r => r.UserVisitedShop).Where(y => y.UserVisitedShop.Select(t => t.UserId).Contains(spec.uservisited));
            }
            return View(await query.ToListAsync());
        }

        /// <summary>
        /// Gallery view of the items
        /// </summary>
        /// <param name="spec">filter the list</param>
        /// <returns></returns>
        public async Task<ActionResult> ItemsList(ItemSpecification spec)
        {
            IQueryable<Items> query = db.Items;
            if (spec.shopid != null)
            {
                query = query.Where(i => i.ShopId == spec.shopid);
            }
            if(!string.IsNullOrEmpty(spec.ownerid))
            {
                var shopIds = await db.Shops.Where(t => t.OwnerId == spec.ownerid).Select(u => u.Id).ToListAsync(); ;
                query = query.Where(i => shopIds.Contains((int)i.ShopId));    
            }
            if (spec.itemTypeId != null)
            {
                query = query.Where(i => i.TypeId == spec.itemTypeId);
            }
            if (spec.sizeids != null && spec.sizeids.Length > 0)
            {
                foreach (var size in spec.sizeids)
                {
                    if (size.HasValue)
                    {
                        query = query.Where(i => i.Sizes.Select(t => t.Id).ToList().Contains((int)size));
                    }
                }
            }
            if (spec.colorids != null && spec.colorids.Length > 0)
            {
                foreach (var color in spec.colorids)
                {
                    if (color.HasValue)
                    {
                        query = query.Where(i => i.Sizes.Select(t => t.Id).ToList().Contains((int)color));
                    }
                }
            }
            if (!string.IsNullOrEmpty(spec.userliked))
            {
                query = query.Include(r=>r.UserLikedItem).Where(i=>i.UserLikedItem.Where(r=>r.IsActive).Select(u=>u.UserId).Contains(spec.userliked));
            }
            if (!string.IsNullOrEmpty(spec.uservisited))
            {
                query = query.Include(r => r.UserVistedItem).Where(y=>y.UserVistedItem.Select(t=>t.UserId).Contains(spec.uservisited));
            }
            return View(await query.ToListAsync());
        }

        /// <summary>
        /// Show to the authorized users who liked a shop
        /// </summary>
        /// <param name="id">the shop</param>
        /// <returns></returns>
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> ShopLikedBy(int id)
        {
            var users = await db.AspNetUsers.Include(u => u.UserLikeShop).Where(t => t.UserLikeShop.Where(r=>r.IsActive).Select(r => r.ShopId).Contains(id)).ToListAsync();
            return View(Mapper.Map<List<UserViewModel>>(users));
        }

        /// <summary>
        /// Show to the authorized users who visited a shop
        /// </summary>
        /// <param name="id">the shop</param>
        /// <returns></returns>
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> ShopVisitedBy(int id)
        {
            return View(Mapper.Map<List<UserViewModel>>(await db.AspNetUsers.Include(u => u.UserVisitedShop).Where(t => t.UserVisitedShop.Select(r => r.ShopId).Contains(id)).ToListAsync()));
        }

        /// <summary>
        /// Show to the authorized users who liked an item
        /// </summary>
        /// <param name="id">the item</param>
        /// <returns></returns>
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> ItemLikedBy(int id)
        {
            var users = await db.AspNetUsers.Include(u => u.UserLikedItem).Where(t => t.UserLikedItem.Where(r => r.IsActive).Select(r => r.ItemId).Contains(id)).ToListAsync();
            return View(Mapper.Map<List<UserViewModel>>(users));
        }

        /// <summary>
        /// Show to the authorized users who visited an item
        /// </summary>
        /// <param name="id">the item</param>
        /// <returns></returns>
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> ItemVisitedBy(int id)
        {
            return View(Mapper.Map<List<UserViewModel>>(await db.AspNetUsers.Include(u => u.UserVistedItem).Where(t => t.UserVistedItem.Select(r => r.ItemId).Contains(id)).ToListAsync()));
        }

    }
}