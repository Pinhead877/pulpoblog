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

namespace ButiqueShops.Controllers
{
    public class ShopsController : Controller
    {

        private ButiqueShopsEntities db = new ButiqueShopsEntities();

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
            return View(shops);
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
        public async Task<ActionResult> Create(Shops shops)
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
                db.Shops.Add(shops);
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
            return View(shops);
        }

        // POST: Shops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles = "Administrator, Shop Owner")]
        public async Task<ActionResult> Edit(Shops shops)
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
                db.Entry(shops).State = EntityState.Modified;
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
            return View(shops);
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
