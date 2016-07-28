using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ButiqueShops.Controllers
{
    public class ShopController : Controller
    {
        private ButiqueShopsEntities db = new ButiqueShopsEntities();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        // GET: AddShop
        public ActionResult AddShop()
        {
            var users = db.AspNetUsers.ToList();
            var items = new List<SelectListItem>();
            foreach(var user in users)
                items.Add(new SelectListItem{ Text = user.UserName, Value = user.Id});
            ViewBag.OwnerId = items;
            return View();
        }
    }
}