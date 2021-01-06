using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.Controllers
{
    public class BookViewController : Controller
    {
        BookshopEntities db = new BookshopEntities();
        // GET: BookView
        public ActionResult bookCategory()
        {
            var list = db.Categories.Where(m => m.status == 1).OrderBy(m => m.name).Take(3)
                .ToList();
            return View("BookOfCategory2", list);
        }
        public ActionResult bookOfCategory(int catId)
        {
            ViewBag.category = db.Categories.Where(m => m.status == 1).ToList();
            var list = db.Books.Where(m => m.status == 1 && m.catid == catId).OrderByDescending(m => m.pricesale).Take(10)
                .ToList();
            return View("readProduct", list);
        }
    }
}