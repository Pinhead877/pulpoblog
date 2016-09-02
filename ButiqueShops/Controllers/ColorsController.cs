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
    /// <summary>
    /// Controls the Colors in the system
    /// </summary>
    public class ColorsController : Controller
    {
        private ButiqueShopsEntities db = new ButiqueShopsEntities();

        // GET: Colors
        /// <summary>
        /// Show a list of colors
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await db.Colors.ToListAsync());
        }

        // GET: Colors/Details/5
        /// <summary>
        /// Show a page with specific color
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colors colors = await db.Colors.FindAsync(id);
            if (colors == null)
            {
                return HttpNotFound();
            }
            return View(colors);
        }

        // GET: Colors/Create
        /// <summary>
        /// Shows the page of create new color
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Colors/Create
        /// <summary>
        /// Save a color in the db
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Colors colors)
        {
            if (ModelState.IsValid)
            {
                db.Colors.Add(colors);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(colors);
        }

        // GET: Colors/Edit/5
        /// <summary>
        /// returns the edit page for color
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colors colors = await db.Colors.FindAsync(id);
            if (colors == null)
            {
                return HttpNotFound();
            }
            return View(colors);
        }

        // POST: Colors/Edit/5
        /// <summary>
        /// saves the edited color in the db
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Colors colors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(colors).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(colors);
        }

        // GET: Colors/Delete/5
        /// <summary>
        /// shows a conformation page before deleting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colors colors = await db.Colors.FindAsync(id);
            if (colors == null)
            {
                return HttpNotFound();
            }
            return View(colors);
        }

        // POST: Colors/Delete/5
        /// <summary>
        /// deletes the color from the system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Colors colors = await db.Colors.FindAsync(id);
            db.Colors.Remove(colors);
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
