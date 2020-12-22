using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        BookshopEntities db = new BookshopEntities();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.product = db.Books.Count();
            ViewBag.Neworder = db.Orders.Where(m => m.status == 2).Count();
            ViewBag.contact = db.Contacts.Where(m => m.status == 2).Count();
            ViewBag.user = db.Users.Where(m => m.status == 1).Count();
            ViewBag.category = db.Categories.Count();
            ViewBag.post = db.Posts.Count();
            ViewBag.topic = db.Topics.Count();
            ViewBag.slider = db.Sliders.Count();
            return View();
        }
    }
}