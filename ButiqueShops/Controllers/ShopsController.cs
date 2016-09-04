using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ButiqueShops.Models;
using ButiqueShops.Extensions;
using System.IO;
using ButiqueShops.ViewModels;
using AutoMapper;

namespace ButiqueShops.Controllers
{
    public class ShopsController : Controller
    {
        private ButiqueShopsEntities db;
        public ShopsController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }

        // GET: Shops
        [AuthorizeRoles(Roles = "Administrator")]
        public async Task<ActionResult> Index()
        {
            var shops = db.Shops.Include(s => s.AspNetUsers);
            return View(await shops.ToListAsync());
        }

        // GET: Shops/Details/5
        [AuthorizeRoles(Roles = "Administrator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shops shops = await db.Shops.FindAsync(id);
            if (shops == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<ShopViewModel>(shops));
        }

        // GET: Shops/Create
        [AuthorizeRoles(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View();
        }

        // POST: Shops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles = "Administrator")]
        public async Task<ActionResult> Create(ShopViewModel shops)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files["fileLogo"];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(path);
                shops.LogoPath = "/Images/" + fileName;
            }
            if (ModelState.IsValid)
            {
                shops.DateAdded = DateTime.Now;
                db.Shops.Add(Mapper.Map<Shops>(shops));
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName", shops.OwnerId);
            return View(shops);
        }

        // GET: Shops/Edit/5
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shops shops = await db.Shops.Include(u => u.AspNetUsers).FirstOrDefaultAsync(u => u.Id == id);
            if (shops == null)
            {
                return HttpNotFound();
            }
            else if (!User.IsInRole("Administrator") && shops.AspNetUsers.UserName != User.Identity.Name)
            {
                return View("Unauthorized");
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName", shops.OwnerId);
            return View(Mapper.Map<ShopViewModel>(shops));
        }

        // POST: Shops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> Edit(ShopViewModel shops)
        {
            if (Request.Files.Count > 0)
            {
                if (Request.Files["fileLogo"].ContentLength > 0)
                {
                    var file = Request.Files["fileLogo"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    shops.LogoPath = "/Images/" + fileName;
                }
            }
            if (ModelState.IsValid)
            {
                if(User.IsInRole("Shop Owner"))
                {
                    var user = await db.AspNetUsers.FirstOrDefaultAsync(u=>u.UserName==User.Identity.Name);
                    shops.OwnerId = user.Id;
                }
                var shopsModel = Mapper.Map<Shops>(shops);
                db.Entry(shopsModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (User.IsInRole("Administrator"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("UserProfile", "Account");
                }
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName", shops.OwnerId);
            return View(shops);
        }

        // GET: Shops/Delete/5
        [AuthorizeRoles(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shops shops = await db.Shops.Include(shop => shop.Items).Where(s => s.Id == id).FirstOrDefaultAsync();
            if (shops == null)
            {
                return HttpNotFound();
            }
            else if (shops.Items.Count > 0)
            {
                ViewBag.ErrorTitle = "Can't Delete";
                ViewBag.ErrorMessage = "The system cannot delete a shop that has items. Please delete the items first and try again.";
                return View("Error");
            }
            return View(Mapper.Map<ShopViewModel>(shops));
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Shops shops = await db.Shops.FindAsync(id);
            db.Shops.Remove(shops);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> TopLiked()
        {
            var shopsIdsLiked = await db.UserLikeShop.Where(r=>r.IsActive).GroupBy(t => t.ShopId).Select(g => new { ShopId = g.Key, LikeCount = g.Count() }).OrderByDescending(f => f.LikeCount).Take(10).ToListAsync();
            var shopsLiked = new List<ShopViewModel>();
            for (int i = 0; i < shopsIdsLiked.Count; i++)
            {
                var id = shopsIdsLiked[i].ShopId;
                var shop = await db.Shops.FirstOrDefaultAsync(g => g.Id == id);
                var shopsViewModel = Mapper.Map<ShopViewModel>(shop);
                shopsViewModel.NumOfLikes = shopsIdsLiked[i].LikeCount;
                shopsLiked.Add(shopsViewModel);
            }
            return View(shopsLiked);
        }

        public async Task<ActionResult> TopVisited()
        {
            var shopsIdsVisited = await db.UserVisitedShop.GroupBy(t => t.ShopId).Select(g => new { ShopId = g.Key, VisitCount = g.Count() }).OrderByDescending(f => f.VisitCount).Take(10).ToListAsync();
            var shopsVisited = new List<ShopViewModel>();
            for (int i=0;i< shopsIdsVisited.Count;i++)
            {
                var id = shopsIdsVisited[i].ShopId;
                var shop = await db.Shops.FirstOrDefaultAsync(g => g.Id == id);
                var shopsViewModel = Mapper.Map<ShopViewModel>(shop);
                shopsViewModel.NumOfVisits = shopsIdsVisited[i].VisitCount;
                shopsVisited.Add(shopsViewModel);
            }
            return View(shopsVisited);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
