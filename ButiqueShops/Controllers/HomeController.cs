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
    /// <summary>
    /// main page and pages alike
    /// </summary>
    public class HomeController : Controller
    {
        private ButiqueShopsEntities db;
        /// <summary>
        /// contructor
        /// </summary>
        public HomeController()
        {
            db = new ButiqueShopsEntities();
        }
        /// <summary>
        /// shows the index page
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await db.Items.Where(i=>i.IsFeatured).ToListAsync());
        }

        /// <summary>
        /// shows the about page
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {

            return View();
        }

        //public ActionResult Contact()
        //{

        //    return View();
        //}


        /// <summary>
        /// Opens the help page
        /// </summary>
        /// <returns></returns>
        public ActionResult Help()
        {
            return View();
        }
    }
}