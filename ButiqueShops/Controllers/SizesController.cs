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
using ButiqueShops.Extensions;

namespace ButiqueShops.Controllers
{
    /// <summary>
    /// CRUD for sizes
    /// </summary>
    [AuthorizeRoles(Roles = "Administrator")]
    public class SizesController : Controller
    {
        private ButiqueShopsEntities db;
        /// <summary>
        /// constructor
        /// </summary>
        public SizesController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }
        // GET: Sizes
        /// <summary>
        /// sizes list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(Mapper.Map<List<SizesViewModel>>(await db.Sizes.ToListAsync()));
        }

        // GET: Sizes/Details/5
        /// <summary>
        /// side details page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sizes sizes = await db.Sizes.FindAsync(id);
            if (sizes == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<SizesViewModel>(sizes));
        }

        // GET: Sizes/Create
        /// <summary>
        /// crearte page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// create a new size in the db
        /// </summary>
        /// <param name="sizes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FullName,ShortName")] SizesViewModel sizes)
        {
            if (ModelState.IsValid)
            {
                db.Sizes.Add(Mapper.Map<Sizes>(sizes));
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sizes);
        }

        // GET: Sizes/Edit/5
        /// <summary>
        /// edit page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sizes sizes = await db.Sizes.FindAsync(id);
            if (sizes == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<SizesViewModel>(sizes));
        }

        // POST: Sizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// updates a size in the db
        /// </summary>
        /// <param name="sizes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FullName,ShortName")] SizesViewModel sizes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Mapper.Map<Sizes>(sizes)).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sizes);
        }

        // GET: Sizes/Delete/5
        /// <summary>
        /// size delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sizes sizes = await db.Sizes.FindAsync(id);
            if (sizes == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<SizesViewModel>(sizes));
        }

        // POST: Sizes/Delete/5
        /// <summary>
        /// deletes a size from the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sizes sizes = await db.Sizes.FindAsync(id);
            db.Sizes.Remove(sizes);
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
