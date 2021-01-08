using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using BookstoreMVC5.library;

namespace BookstoreMVC5.Controllers
{
    public class ModulesController : Controller
    {
        // GET: Modules
        BookshopEntities db = new BookshopEntities();
        public ActionResult Header()
        {
            //Session.Add(USER_SESSION, user);
            ViewBag.username = null;
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.username = sessionUser.username;
                ViewBag.ID = sessionUser.ID;
                ViewBag.fullname = sessionUser.fullname;
                ViewBag.status = sessionUser.status;
            }
            return View("Header");
        }

        public ActionResult Slider()
        {
            var list = db.Sliders.Where(m => m.status == 1).OrderBy(m => m.sort).ToList();
            //return View("_slider", list);
            return View("Slidershow", list);
        }

        public ActionResult ADS()
        {
            return View("ADS");
        }

        public ActionResult Category()
        {
            var listCate = db.Categories.Where(m => m.status == 1).OrderBy(m => m.sort).ToList();
            //return View("_category", listCate);
            return View("Category", listCate);
        }

        public ActionResult Blog()
        {
            var listblog = db.Topics.Where(m => m.status == 1).OrderBy(m => m.sort).ToList();
            return View("ListBlog", listblog);
        }

        public ActionResult MobileCategory()
        {
            var listCate = db.Categories.Where(m => m.status == 1).OrderBy(m => m.sort).ToList();
            //return View("_category", listCate);
            return View("MobileCategory", listCate);
        }

        public ActionResult Mainmenu()
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.ID = sessionUser.ID;
            }
            return View("Mainmenu");

        }

        public ActionResult footer()
        {
            return View("Footer");
        }
    }
}