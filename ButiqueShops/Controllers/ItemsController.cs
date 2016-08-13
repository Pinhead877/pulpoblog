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
using ButiqueShops.ViewModels;
using AutoMapper;

namespace ButiqueShops.Controllers
{
    public class ItemsController : Controller
    {
        private ButiqueShopsEntities db;

        public ItemsController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }

        // GET: Items
        public async Task<ActionResult> Index(int? shopId)
        {
            IQueryable<Items> query = db.Items.Include(i => i.ItemTypes).Include(i => i.Shops);
            if (shopId != null)
            {
                query = query.Where(item => item.ShopId == shopId);
                ViewBag.shopId = shopId;
                ViewBag.shopName = db.Shops.Where(shop => shop.Id == shopId).FirstOrDefault().Name;
            }
            return View(await query.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = await db.Items.Include(r=>r.Sizes).Where(s=>s.Id==id).FirstOrDefaultAsync();
            if (items == null)
            {
                return HttpNotFound();
            }
            var itemsViewModel = Mapper.Map<ItemsViewModel>(items);
            ViewBag.Sizes = new MultiSelectList(itemsViewModel.Sizes, "Id", "ShortName");
            ViewBag.Colors = new MultiSelectList(itemsViewModel.Colors, "Id", "Name");
            return View(itemsViewModel);
        }

        // GET: Items/Create
        public async Task<ActionResult> Create(int? shopId)
        {
            var shops = db.Shops.ToList();
            if(shops.Count==0)
            {
                ViewBag.ErrorTitle = "No Shops";
                ViewBag.ErrorMessage = "Please add shops to the system.";
                return View("Error");
            }
            if (shopId != null)
            {
                ViewBag.ReturnUrl = true;
            }
            Shops selectedItem = shopId==null ? null : await db.Shops.Where(shop=>shop.Id==shopId).FirstOrDefaultAsync();
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", selectedItem);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name"); ;
            ViewBag.colorsIds = new MultiSelectList(db.Colors.OrderBy(r=>r.Name), "Id", "Name");
            ViewBag.sizesIds = new MultiSelectList(db.Sizes, "Id", "ShortName"); ;
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemsViewModel items)
        {
            var sizesFromDB = db.Sizes.ToList();
            var colorsFromDB = db.Colors.ToList();

            foreach(var size in items.sizesIds)
            {
                items.Sizes = (items.Sizes==null)? new List<Sizes>():items.Sizes;
                items.Sizes.Add(sizesFromDB.FirstOrDefault(s => s.Id == size));
            }
            foreach (var color in items.colorsIds)
            {
                items.Colors = items.Colors == null ? new List<Colors>() : items.Colors;
                items.Colors.Add(colorsFromDB.FirstOrDefault(s => s.Id == color));
            }
            if (IsItemValidated(items))
            {
                var itemsModel = Mapper.Map<Items>(items);
                db.Items.Add(itemsModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", items.TypeId);
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", items.ShopId);
            ViewBag.colorsIds = new MultiSelectList(colorsFromDB, "Id", "Name", items.Colors.Select(r => r.Id).ToArray());
            ViewBag.sizesIds = new MultiSelectList(sizesFromDB, "Id", "ShortName", items.Sizes.Select(r => r.Id).ToArray());
            return View(items);
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = await db.Items.Include(r=>r.Sizes).Where(s=>s.Id == id).FirstOrDefaultAsync();
            if (items == null)
            {
                return HttpNotFound();
            }
            var itemsViewModel = Mapper.Map<ItemsViewModel>(items);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", itemsViewModel.TypeId);
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", itemsViewModel.ShopId);
            ViewBag.colorsIds = new MultiSelectList(db.Colors, "Id", "Name", items.Colors.Select(r => r.Id).ToArray());
            ViewBag.sizesIds = new MultiSelectList(db.Sizes, "Id", "ShortName", items.Sizes.Select(r => r.Id).ToArray());
            return View(itemsViewModel);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ItemsViewModel items)
        {
            var sizesFromDB = db.Sizes.ToList();
            var colorsFromDB = db.Colors.ToList();

            foreach (var size in items.sizesIds)
            {
                items.Sizes = (items.Sizes == null) ? new List<Sizes>() : items.Sizes;
                items.Sizes.Add(sizesFromDB.FirstOrDefault(s => s.Id == size));
            }
            foreach (var color in items.colorsIds)
            {
                items.Colors = items.Colors == null ? new List<Colors>() : items.Colors;
                items.Colors.Add(colorsFromDB.FirstOrDefault(s => s.Id == color));
            }
            if (IsItemValidated(items))
            {
                var itemsModel = Mapper.Map<Items>(items);
                db.Entry(itemsModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Name", items.TypeId);
            ViewBag.ShopId = new SelectList(db.Shops, "Id", "Name", items.ShopId);
            ViewBag.Colors = new MultiSelectList(colorsFromDB, "Id", "Name", items.Colors.Select(r => r.Id).ToArray());
            ViewBag.Sizes = new MultiSelectList(sizesFromDB, "Id", "ShortName", items.Sizes);
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

        private bool IsItemValidated(ItemsViewModel items)
        {
            return true;
        }
    }
}
