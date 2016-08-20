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
using AutoMapper;

namespace ButiqueShops.Controllers
{
    [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
    public class ItemsToSubmitController : Controller
    {
        private ButiqueShopsEntities db;

        public ItemsToSubmitController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }

        // GET: ItemsToSubmits
        public async Task<ActionResult> Index()
        {
            IQueryable<ItemsToSubmit> query = db.ItemsToSubmit.Include(i => i.Shops).Include(i => i.ItemTypes);
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                query = query.Where(s=>s.Shops.OwnerId==shopOwner.Id);
            }
            return View(await query.ToListAsync());
        }

        // GET: ItemsToSubmits/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemsToSubmit itemsToSubmit = await db.ItemsToSubmit.FindAsync(id);
            if (itemsToSubmit == null)
            {
                return HttpNotFound();
            }
            return View(itemsToSubmit);
        }

        // GET: ItemsToSubmits/Create
        public async Task<ActionResult> Create()
        {
            IQueryable<Shops> query = db.Shops;
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                query = query.Where(u=>u.OwnerId==shopOwner.Id);
            }
            ViewBag.ShopId = new SelectList(query, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name");
            return View();
        }

        // POST: ItemsToSubmits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemsToSubmit itemsToSubmit)
        {
            if (Request.Files.Count > 0)
            {
                if (Request.Files["fileBig"].ContentLength > 0)
                {
                    var file = Request.Files["fileBig"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    itemsToSubmit.ImagePath = "/Images/" + fileName;
                }
                if (Request.Files["fileSmall"].ContentLength > 0)
                {
                    var file = Request.Files["fileSmall"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    itemsToSubmit.SmallImagePath = "/Images/" + fileName;
                }
            }
            if (ModelState.IsValid)
            {
                db.ItemsToSubmit.Add(itemsToSubmit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            IQueryable<Shops> query = db.Shops;
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                query = query.Where(u => u.OwnerId == shopOwner.Id);
            }
            ViewBag.ShopId = new SelectList(query, "Id", "Name", itemsToSubmit.ShopId);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", itemsToSubmit.TypeId);
            return View(itemsToSubmit);
        }

        // GET: ItemsToSubmits/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemsToSubmit itemsToSubmit = await db.ItemsToSubmit.FindAsync(id);
            if (itemsToSubmit == null)
            {
                return HttpNotFound();
            }
            IQueryable<Shops> query = db.Shops;
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                query = query.Where(u => u.OwnerId == shopOwner.Id);
            }
            ViewBag.ShopId = new SelectList(query, "Id", "Name", itemsToSubmit.ShopId);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", itemsToSubmit.TypeId);
            return View(itemsToSubmit);
        }

        // POST: ItemsToSubmits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ItemsToSubmit itemsToSubmit)
        {
            if (Request.Files.Count > 0)
            {
                if (Request.Files["fileBig"].ContentLength > 0)
                {
                    var file = Request.Files["fileBig"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    itemsToSubmit.ImagePath = "/Images/" + fileName;
                }
                if (Request.Files["fileSmall"].ContentLength > 0)
                {
                    var file = Request.Files["fileSmall"];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    itemsToSubmit.SmallImagePath = "/Images/" + fileName;
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(itemsToSubmit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            IQueryable<Shops> query = db.Shops;
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                query = query.Where(u => u.OwnerId == shopOwner.Id);
            }
            ViewBag.ShopId = new SelectList(query, "Id", "Name", itemsToSubmit.ShopId);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", itemsToSubmit.TypeId);
            return View(itemsToSubmit);
        }

        // GET: ItemsToSubmits/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemsToSubmit itemsToSubmit = await db.ItemsToSubmit.Include(u=>u.Shops).FirstOrDefaultAsync(y=>y.Id==id);
            if (itemsToSubmit == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if(itemsToSubmit.Shops.OwnerId != shopOwner.Id)
                {
                    return View("Unauthorized");
                }
            }
            return View(itemsToSubmit);
        }

        // POST: ItemsToSubmits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ItemsToSubmit itemsToSubmit = await db.ItemsToSubmit.FindAsync(id);
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (itemsToSubmit.Shops.OwnerId != shopOwner.Id)
                {
                    return View("Unauthorized");
                }
            }
            db.ItemsToSubmit.Remove(itemsToSubmit);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(Roles = "Administrator")]
        public async Task<ActionResult> Approve(int id)
        {
            var itemToSubmit = await db.ItemsToSubmit.FirstOrDefaultAsync(t => t.Id == id);
            db.Items.Add(Mapper.Map<Items>(itemToSubmit));
            db.ItemsToSubmit.Remove(itemToSubmit);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
