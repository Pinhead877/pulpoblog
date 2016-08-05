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

namespace ButiqueShops.Controllers
{
    public class ItemsController : Controller
    {
        private ButiqueShopsEntities db = new ButiqueShopsEntities();

        // GET: Items
        public async Task<ActionResult> Index()
        {
            var items = db.Items.Include(i => i.ItemTypes).Include(i => i.Shops);
            return View(await items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = await db.Items.FindAsync(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // GET: Items/Create
        public async Task<ActionResult> Create(int? shopId)
        {
            //TODO  - use the shopId to select the shop
            var shops = await db.Shops.ToListAsync();
            if(shops.Count==0)
            {
                ViewBag.ErrorTitle = "No Shops";
                ViewBag.ErrorMessage = "Please add shops to the system.";
                return View("Error");
            }
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name");
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,TypeId,Price,Quantity,Color,Size,ShopId,DateAdded")] Items items)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(items);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", items.TypeId);
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", items.ShopId);
            return View(items);
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = await db.Items.FindAsync(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", items.TypeId);
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", items.ShopId);
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,TypeId,Price,Quantity,Color,Size,ShopId,DateAdded")] Items items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(items).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", items.TypeId);
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", items.ShopId);
            return View(items);
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = await db.Items.FindAsync(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Items items = await db.Items.FindAsync(id);
            db.Items.Remove(items);
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
