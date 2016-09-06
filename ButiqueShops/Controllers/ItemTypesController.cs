using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ButiqueShops.Models;
using ButiqueShops.Extensions;

namespace ButiqueShops.Controllers
{
    /// <summary>
    /// CRUD for itemtype for admin
    /// </summary>
    [AuthorizeRoles(Roles = "Administrator")]
    public class ItemTypesController : Controller
    {
        private ButiqueShopsEntities db = new ButiqueShopsEntities();

        // GET: ItemTypes
        /// <summary>
        /// item types list
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.ItemTypes.ToList());
        }

        // GET: ItemTypes/Details/5
        /// <summary>
        /// item type details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            if (itemTypes == null)
            {
                return HttpNotFound();
            }
            return View(itemTypes);
        }

        // GET: ItemTypes/Create
        /// <summary>
        /// create page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// creates new itemtype in the db
        /// </summary>
        /// <param name="itemTypes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ItemTypes itemTypes)
        {
            if (ModelState.IsValid)
            {
                db.ItemTypes.Add(itemTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itemTypes);
        }

        // GET: ItemTypes/Edit/5
        /// <summary>
        /// edit page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            if (itemTypes == null)
            {
                return HttpNotFound();
            }
            return View(itemTypes);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// updates the itemtype in the db
        /// </summary>
        /// <param name="itemTypes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ItemTypes itemTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemTypes);
        }

        // GET: ItemTypes/Delete/5
        /// <summary>
        /// item type delete confirmaion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            if (itemTypes == null)
            {
                return HttpNotFound();
            }
            return View(itemTypes);
        }

        // POST: ItemTypes/Delete/5
        /// <summary>
        /// deletes an item type from the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemTypes itemTypes = db.ItemTypes.Find(id);
            db.ItemTypes.Remove(itemTypes);
            db.SaveChanges();
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
