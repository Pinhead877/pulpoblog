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
    public class SizesController : Controller
    {
        private ButiqueShopsEntities db;
        public SizesController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }
        // GET: Sizes
        public async Task<ActionResult> Index()
        {
            return View(Mapper.Map<List<SizesViewModel>>(await db.Sizes.ToListAsync()));
        }

        // GET: Sizes/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
