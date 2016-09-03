using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ButiqueShops.Controllers
{
    public class HomeController : Controller
    {
        private ButiqueShopsEntities db;
        public HomeController()
        {
            db = new ButiqueShopsEntities();
        }
        public async Task<ActionResult> Index()
        {
            return View(await db.Items.Where(i=>i.IsFeatured).ToListAsync());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}