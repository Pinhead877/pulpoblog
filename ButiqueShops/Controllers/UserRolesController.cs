using AutoMapper;
using ButiqueShops.Extensions;
using ButiqueShops.Models;
using ButiqueShops.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ButiqueShops.Controllers
{
    /// <summary>
    /// CRUD for user roles 
    /// </summary>
    [AuthorizeRoles(Roles = "Administrator")]
    public class UserRolesController : Controller
    {
        private ButiqueShopsEntities db;
        /// <summary>
        /// constructor
        /// </summary>
        public UserRolesController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }

        /// <summary>
        /// user roles list page
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var usersRoles = await db.AspNetUsers.Include(r => r.AspNetRoles).Where(u => u.AspNetRoles.Any()).ToListAsync();
            var usersRolesViewModel = Mapper.Map<List<UserViewModel>>(usersRoles);
            return View(usersRolesViewModel);
        }

        /// <summary>
        /// user role details page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            return View();
        }

        /// <summary>
        /// create page
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            var users = db.AspNetUsers.ToListAsync();
            var roles = new ButiqueShopsEntities().AspNetRoles.ToListAsync();
            await Task.WhenAll(users, roles);
            var usersItems = new List<SelectListItem>();
            usersItems.Add(new SelectListItem { Text = "", Value = null });
            foreach (var user in users.Result)
                usersItems.Add(new SelectListItem { Text = user.UserName, Value = user.Id });
            var rolesItems = new List<SelectListItem>();
            rolesItems.Add(new SelectListItem { Text = "", Value = null });
            foreach (var role in roles.Result)
                rolesItems.Add(new SelectListItem { Text = role.Name, Value = role.Id.ToString() });
            ViewBag.UserId = usersItems;
            ViewBag.RoleId = rolesItems;
            return View();
        }

        /// <summary>
        /// creates new user role in the db
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(UserRolesViewModel userRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await db.AspNetUsers.Where(u => u.Id == userRole.UserId).FirstOrDefaultAsync();
                    var role = await db.AspNetRoles.Where(r => r.Id == userRole.RoleId).FirstOrDefaultAsync();
                    user.AspNetRoles.Clear();
                    user.AspNetRoles.Add(role);
                    db.AspNetUsers.Attach(user);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(userRole);
            }
            catch (Exception e)
            {
                ViewBag.ErrorTitle = "Error";
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// edit user role page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            var user = await db.AspNetUsers.Include(u => u.AspNetRoles).FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", user.Id);
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", user.AspNetRoles.ToList()[0].Id);
            return View();
        }

        /// <summary>
        /// updates user role in the db
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(UserRolesViewModel userRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await db.AspNetUsers.Where(u => u.Id == userRole.UserId).FirstOrDefaultAsync();
                    var role = await db.AspNetRoles.Where(r => r.Id == userRole.RoleId).FirstOrDefaultAsync();
                    user.AspNetRoles.Clear();
                    user.AspNetRoles.Add(role);
                    db.AspNetUsers.Attach(user);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorTitle = "Error";
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }
        /// <summary>
        /// delete conforamtion page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(string id)
        {
            var user = await db.AspNetUsers.Include(u => u.AspNetRoles).FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.UserId = user.UserName;
            ViewBag.RoleId = user.AspNetRoles.ToList()[0].Name;
            return View();
        }

        /// <summary>
        /// delete a user role from the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await db.AspNetUsers.Include(u => u.AspNetRoles).FirstOrDefaultAsync(u => u.Id == id);
                user.AspNetRoles.Clear();
                db.AspNetUsers.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("error");
            }
        }
    }
}
