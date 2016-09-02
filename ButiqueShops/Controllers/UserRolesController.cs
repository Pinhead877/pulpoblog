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
    public class UserRolesController : Controller
    {

        private ButiqueShopsEntities db;
        public UserRolesController()
        {
            db = new ButiqueShopsEntities();
            AutoMapperConfig.Config();
        }

        public async Task<ActionResult> Index()
        {
            var usersRoles = await db.AspNetUsers.Include(r => r.AspNetRoles).Where(u => u.AspNetRoles.Any()).ToListAsync();
            var usersRolesViewModel = Mapper.Map<List<UserViewModel>>(usersRoles);
            return View(usersRolesViewModel);
        }

        public ActionResult Details(string id)
        {
            return View();
        }

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

        public async Task<ActionResult> Edit(string id)
        {
            var user = await db.AspNetUsers.Include(u => u.AspNetRoles).FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", user.Id);
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", user.AspNetRoles.ToList()[0].Id);
            return View();
        }

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

        public async Task<ActionResult> Delete(string id)
        {
            var user = await db.AspNetUsers.Include(u => u.AspNetRoles).FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.UserId = user.UserName;
            ViewBag.RoleId = user.AspNetRoles.ToList()[0].Name;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles = "Administrator")]
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
