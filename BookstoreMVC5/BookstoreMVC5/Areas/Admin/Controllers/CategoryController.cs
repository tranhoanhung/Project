using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private BookshopEntities db = new BookshopEntities();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var list = db.Categories.Where(m => m.status != 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.listcategory = db.Categories.Where(m => m.status != 0).ToList();
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(category.name.ToString());
                category.slug = slug;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listcategory = db.Categories.Where(m => m.status != 0).ToList();
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.listcategory = db.Categories.Where(m => m.status != 0).ToList();
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(category.name.ToString());
                category.slug = slug;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listcategory = db.Categories.Where(m => m.status != 0).ToList();
            return View(category);
        }
        public ActionResult Status(int id)
        {
            Category category = db.Categories.Find(id);
            category.status = (category.status == 1) ? 2 : 1;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.Categories.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            Category category = db.Categories.Find(id);
            category.status = 0;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Retrash(int id)
        {
            Category category = db.Categories.Find(id);
            category.status = 2;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("trash");
        }

        public ActionResult deleteTrash(int id)
        {
            Category mcategory = db.Categories.Find(id);
            db.Categories.Remove(mcategory);
            db.SaveChanges();
            Message.set_flash("Đã xóa vĩnh viễn 1 sản phẩm", "success");
            return RedirectToAction("trash");
        }


    }
}
