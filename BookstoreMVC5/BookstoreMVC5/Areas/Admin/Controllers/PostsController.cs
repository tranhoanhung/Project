using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private BookshopEntities db = new BookshopEntities();

        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Topic);
            var list = db.Posts.Where(m => m.status > 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Create()
        {
            ViewBag.topid = new SelectList(db.Topics, "ID", "name");
            ViewBag.listTopic = db.Topics.Where(m => m.status != 0).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Post post )
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file;
                var namecateDb = db.Topics.Where(m => m.ID == post.topid).First();
                string slug = Mystring.ToSlug(post.title.ToString());
                string namecate = Mystring.ToStringNospace(namecateDb.name);
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                string ExtensionFile = Mystring.GetFileExtension(filename);
                string namefilenew = namecate + "/" + slug + "." + ExtensionFile;
                var path = Path.Combine(Server.MapPath("~/Public/images/post"), namefilenew);
                var folder = Server.MapPath("~/Public/images/post/" + namecate);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                file.SaveAs(path);
                post.img = namefilenew;
                post.slug = slug;
                post.date_created = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                Message.set_flash("Thêm thành công", "success");
                return RedirectToAction("Index");
            }
            ViewBag.topid = new SelectList(db.Topics, "ID", "name", post.topid);
            return View(post);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.listTopic = db.Topics.Where(m => m.status == 1).ToList();
            ViewBag.topid = new SelectList(db.Topics, "ID", "name", post.topid);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file;
                string slug = Mystring.ToSlug(post.title.ToString());
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                if (filename.Equals("") == false)
                {
                    var namecateDb = db.Topics.Where(m => m.ID == post.topid).First();
                    string namecate = Mystring.ToStringNospace(namecateDb.name);
                    string ExtensionFile = Mystring.GetFileExtension(filename);
                    string namefilenew = namecate + "/" + slug + "." + ExtensionFile;
                    var path = Path.Combine(Server.MapPath("~/Public/images/post"), namefilenew);
                    var folder = Server.MapPath("~/Public/images/post/" + namecate);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    file.SaveAs(path);
                    post.img = namefilenew;
                }
                post.slug = slug;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listTopic = db.Topics.Where(m => m.status == 1).ToList();
            ViewBag.topid = new SelectList(db.Topics, "ID", "name", post.topid);
            return View(post);
        }
        public ActionResult Status(int id)
        {
            Post post = db.Posts.Find(id);
            post.status = (post.status == 1) ? 2 : 1;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult trash()
        {
            var list = db.Posts.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            Post post = db.Posts.Find(id);
            post.status = 0;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Retrash(int id)
        {
            Post post = db.Posts.Find(id);
            post.status = 2;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("khôi phục thành công", "success");
            return RedirectToAction("trash");
        }
    }
}
