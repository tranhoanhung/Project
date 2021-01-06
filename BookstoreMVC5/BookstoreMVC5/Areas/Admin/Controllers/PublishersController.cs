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
    public class PublishersController : Controller
    {
        private BookshopEntities db = new BookshopEntities();

        public ActionResult Index()
        {
            return View(db.Publishers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(publisher.name.ToString());
                publisher.slug = slug;
                db.Publishers.Add(publisher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(publisher.name.ToString());
                publisher.slug = slug;
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        //public ActionResult Status(int id)
        //{
        //    Publisher publisher = db.Publishers.Find(id);
        //    publisher.s = (publisher.status == 1) ? 2 : 1;
        //    db.Entry(slider).State = EntityState.Modified;
        //    db.SaveChanges();
        //    Message.set_flash("Thay đổi trang thái thành công", "success");
        //    return RedirectToAction("Index");
        //}

        //public ActionResult trash()
        //{
        //    var list = db.Sliders.Where(m => m.status == 0).ToList();
        //    return View("Trash", list);
        //}
        //public ActionResult Deltrash(int id)
        //{
        //    Slider slider = db.Sliders.Find(id);
        //    slider.status = 0;
        //    db.Entry(slider).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //public ActionResult Retrash(int id)
        //{
        //    Slider slider = db.Sliders.Find(id);
        //    slider.status = 2;
        //    db.Entry(slider).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("trash");
        //}


        public ActionResult deleteTrash(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            db.Publishers.Remove(publisher);
            db.SaveChanges();
            Message.set_flash("Đã xóa vĩnh viễn 1 sản phẩm", "success");
            return RedirectToAction("trash");
        }
    }
}
