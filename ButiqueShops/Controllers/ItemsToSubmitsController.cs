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
using ButiqueShops.ViewModels;

namespace ButiqueShops.Controllers
{
    /// <summary>
    /// CRUD for the itemsToSubmit
    /// a class for the shop owner to submit items to review
    /// </summary>
    [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
    public class ItemsToSubmitController : Controller
    {
        private ButiqueShopsEntities db;
        /// <summary>
        /// constructor
        /// </summary>
        public ItemsToSubmitController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }

        // GET: ItemsToSubmits
        /// <summary>
        /// items to submit list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            IQueryable<ItemsToSubmit> query = db.ItemsToSubmit.Include(i => i.Shops).Include(i => i.ItemTypes);
            if (User.IsInRole("Shop Owner"))
            {
                var shopOwner = await db.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                query = query.Where(s=>s.Shops.OwnerId==shopOwner.Id);
            }
            var items = await query.ToListAsync();
            var itemsToSubmitViewModel = Mapper.Map<List<ItemsToSubmitViewModel>>(items);
            return View(itemsToSubmitViewModel);
        }

        // GET: ItemsToSubmits/Details/5
        /// <summary>
        /// item to submit details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            return View(Mapper.Map<ItemsToSubmitViewModel>(itemsToSubmit));
        }

        // GET: ItemsToSubmits/Create
        /// <summary>
        /// create page for item to submit
        /// </summary>
        /// <returns></returns>
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
            ViewBag.colorsIds = new MultiSelectList(db.Colors.OrderBy(r => r.Name), "Id", "Name");
            ViewBag.sizesIds = new MultiSelectList(db.Sizes, "Id", "ShortName");
            return View();
        }

        // POST: ItemsToSubmits/Create
        /// <summary>
        /// create a new item to submit in the db
        /// </summary>
        /// <param name="itemsToSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemsToSubmitViewModel itemsToSubmit)
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
            var sizesFromDB = await db.Sizes.ToListAsync();
            var colorsFromDB = await db.Colors.ToListAsync();
            if (ModelState.IsValid)
            {
                if (itemsToSubmit.sizesIds != null)
                {
                    foreach (var size in itemsToSubmit.sizesIds)
                    {
                        itemsToSubmit.Sizes = (itemsToSubmit.Sizes == null) ? new List<Sizes>() : itemsToSubmit.Sizes;
                        itemsToSubmit.Sizes.Add(sizesFromDB.FirstOrDefault(s => s.Id == size));
                    }

                }
                if (itemsToSubmit.colorsIds != null)
                {
                    foreach (var color in itemsToSubmit.colorsIds)
                    {
                        itemsToSubmit.Colors = itemsToSubmit.Colors == null ? new List<Colors>() : itemsToSubmit.Colors;
                        itemsToSubmit.Colors.Add(colorsFromDB.FirstOrDefault(s => s.Id == color));
                    }
                }
                var itemsToSubmitModel = Mapper.Map<ItemsToSubmit>(itemsToSubmit);
                db.ItemsToSubmit.Add(itemsToSubmitModel);
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
            ViewBag.colorsIds = new MultiSelectList(db.Colors.OrderBy(r => r.Name), "Id", "Name", itemsToSubmit.Colors.Select(r => r.Id).ToArray());
            ViewBag.sizesIds = new MultiSelectList(sizesFromDB, "Id", "ShortName", itemsToSubmit.Sizes.Select(r => r.Id).ToArray());
            return View(itemsToSubmit);
        }

        // GET: ItemsToSubmits/Edit/5
        /// <summary>
        /// opens the edit item to submit page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            ViewBag.colorsIds = new MultiSelectList(db.Colors.OrderBy(r => r.Name), "Id", "Name", itemsToSubmit.Colors.Select(r => r.Id).ToArray());
            ViewBag.sizesIds = new MultiSelectList(db.Sizes, "Id", "ShortName", itemsToSubmit.Sizes.Select(r => r.Id).ToArray());
            return View(Mapper.Map<ItemsToSubmitViewModel>(itemsToSubmit));
        }

        // POST: ItemsToSubmits/Edit/5
        /// <summary>
        /// updates an item to submit
        /// </summary>
        /// <param name="itemsToSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ItemsToSubmitViewModel itemsToSubmit)
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
            var sizesFromDB = db.Sizes.ToList();
            var colorsFromDB = db.Colors.ToList();
            if (ModelState.IsValid)
            {
                if (itemsToSubmit.sizesIds != null)
                {
                    foreach (int size in itemsToSubmit.sizesIds)
                    {
                        itemsToSubmit.Sizes = (itemsToSubmit.Sizes == null) ? new List<Sizes>() : itemsToSubmit.Sizes;
                        itemsToSubmit.Sizes.Add(sizesFromDB.FirstOrDefault(s => s.Id == size));
                    }
                }
                if (itemsToSubmit.colorsIds != null)
                {
                    foreach (int color in itemsToSubmit.colorsIds)
                    {
                        itemsToSubmit.Colors = itemsToSubmit.Colors == null ? new List<Colors>() : itemsToSubmit.Colors;
                        itemsToSubmit.Colors.Add(colorsFromDB.FirstOrDefault(s => s.Id == color));
                    }
                }
                var itemsModel = Mapper.Map<ItemsToSubmit>(itemsToSubmit);
                var itemdb = await db.ItemsToSubmit.FirstOrDefaultAsync(o => o.Id == itemsToSubmit.Id);
                db.Entry(itemdb).Collection(c => c.Colors).Load();
                db.Entry(itemdb).Collection(c => c.Sizes).Load();
                itemdb.ImagePath = itemsModel.ImagePath;
                itemdb.TypeId = itemsToSubmit.TypeId;
                itemdb.Name = itemsToSubmit.Name;
                itemdb.Price = itemsModel.Price;
                itemdb.Quantity = itemsToSubmit.Quantity;
                itemdb.ShopId = itemsToSubmit.ShopId;
                itemdb.SmallImagePath = itemsToSubmit.SmallImagePath;
                itemdb.Colors = new List<Colors>();
                itemdb.Sizes = new List<Sizes>();
                itemdb.Colors = itemsToSubmit.Colors;
                itemdb.Sizes = itemsToSubmit.Sizes;
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
            ViewBag.colorsIds = new MultiSelectList(db.Colors.OrderBy(r => r.Name), "Id", "Name", itemsToSubmit.Colors.Select(r => r.Id).ToArray());
            ViewBag.sizesIds = new MultiSelectList(sizesFromDB, "Id", "ShortName", itemsToSubmit.Sizes);
            return View(itemsToSubmit);
        }

        // GET: ItemsToSubmits/Delete/5
        /// <summary>
        /// item to submit delete confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            return View(Mapper.Map<ItemsToSubmitViewModel>(itemsToSubmit));
        }

        // POST: ItemsToSubmits/Delete/5
        /// <summary>
        /// deletes an item for submit from the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// approve the item to submit by the admin
        /// adds the item to the items list for show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
