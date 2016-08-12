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

namespace ButiqueShops.Controllers
{
    [AuthorizeRoles(Roles = "Administrator")]
    public class ShopsController : Controller
    {

        private ButiqueShopsEntities db = new ButiqueShopsEntities();

        // GET: Shops
        public async Task<ActionResult> Index()
        {
            var shops = db.Shops.Include(s => s.AspNetUsers);
            return View(await shops.ToListAsync());
        }

        // GET: Shops/Details/5
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
            return View(shops);
        }

        // GET: Shops/Create
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View();
        }

        // POST: Shops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Shops shops)
        {
            if (ModelState.IsValid)
            {
                shops.DateAdded = DateTime.Now;
                db.Shops.Add(shops);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName", shops.OwnerId);
            return View(shops);
        }

        // GET: Shops/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName", shops.OwnerId);
            return View(shops);
        }

        // POST: Shops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Shops shops)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shops).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "UserName", shops.OwnerId);
            return View(shops);
        }

        // GET: Shops/Delete/5
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
            }else if (shops.Items.Count > 0)
            {
                ViewBag.ErrorTitle = "Can't Delete";
                ViewBag.ErrorMessage = "The system cannot delete a shop that has items. Please delete the items first and try again.";
                return View("Error");
            }
            return View(shops);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Shops shops = await db.Shops.FindAsync(id);
            db.Shops.Remove(shops);
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
