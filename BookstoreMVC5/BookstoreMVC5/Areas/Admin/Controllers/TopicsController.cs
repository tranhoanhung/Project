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
    public class TopicsController : Controller
    {
        private BookshopEntities db = new BookshopEntities();

        public ActionResult Index()
        {
            var list = db.Topics.Where(m => m.status != 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        public ActionResult Create()
        {
            ViewBag.listtopic = db.Topics.Where(m => m.status != 0).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                //category
                string slug = Mystring.ToSlug(topic.name.ToString());
                topic.slug = slug;
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listtopic = db.Topics.Where(m => m.status != 0).ToList();
            Message.set_flash("Thêm thành công", "success");
            return View(topic);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.listtopic = db.Topics.Where(m => m.status != 0).ToList();
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(topic.name.ToString());
                topic.slug = slug;
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listtopic = db.Topics.Where(m => m.status != 0).ToList();
            return View(topic);
        }

        public ActionResult Status(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.status = (topic.status == 1) ? 2 : 1;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.Topics.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.status = 0;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Retrash(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.status = 2;
            db.Entry(topic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("trash");
        }

        public ActionResult deleteTrash(int id)
        {
            Topic mtopic = db.Topics.Find(id);
            db.Topics.Remove(mtopic);
            db.SaveChanges();
            Message.set_flash("Đã xóa vĩnh viễn 1 Chủ đề", "success");
            return RedirectToAction("trash");
        }

    }
}
