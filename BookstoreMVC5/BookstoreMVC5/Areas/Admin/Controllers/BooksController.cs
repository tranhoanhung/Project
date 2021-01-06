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
    public class BooksController : Controller
    {
        private BookshopEntities db = new BookshopEntities();

        public ActionResult Index()
        {
            var list = db.Books.Where(m => m.status != 0).OrderByDescending(m => m.ID).ToList();
            return View(list);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        public ActionResult Create()
        {
            ViewBag.listCate = db.Categories.Where(m => m.status != 0).ToList();
            ViewBag.catid = new SelectList(db.Categories, "ID", "name");
            ViewBag.publishid = new SelectList(db.Publishers, "ID", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Book book, HttpPostedFileBase file)
        {
            ViewBag.listCate = db.Categories.Where(m => m.status != 0 && m.ID > 2).ToList();
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(book.name.ToString());
                // lấy tên loại sản phẩm
                var namecateDb = db.Categories.Where(m => m.ID == book.catid).First();
                string namecate = Mystring.ToStringNospace(namecateDb.name);
                // lấy tên ảnh
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                //lấy đuôi ảnh
                string ExtensionFile = Mystring.GetFileExtension(filename);
                // lấy tên sản phẩm làm slug

                //lấy tên mới của ảnh slug + [đuôi ảnh lấy đc]
                string namefilenew = namecate + "/" + slug + "." + ExtensionFile;
                //lưu ảnh vào đường đẫn
                var path = Path.Combine(Server.MapPath("~/Public/images/product"), namefilenew);
                //nếu thư mục k tồn tại thì tạo thư mục
                var folder = Server.MapPath("~/Public/images/product/" + namecate);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                file.SaveAs(path);
                book.img = namefilenew;
                book.slug = slug;
                book.date_created = DateTime.Now;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.catid = new SelectList(db.Categories, "ID", "name", book.catid);
            ViewBag.publishid = new SelectList(db.Publishers, "ID", "name", book.publishid);
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.listCate = db.Categories.Where(m => m.status != 0).ToList();
            ViewBag.catid = new SelectList(db.Categories, "ID", "name", book.catid);
            ViewBag.publishid = new SelectList(db.Publishers, "ID", "name", book.publishid);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Book book, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                string slug = Mystring.ToSlug(book.name.ToString());
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                if (filename.Equals("") == false)
                {
                    var namecateDb = db.Categories.Where(m => m.ID == book.catid).First();
                    string namecate = Mystring.ToStringNospace(namecateDb.name);
                    string ExtensionFile = Mystring.GetFileExtension(filename);
                    string namefilenew = namecate + "/" + slug + "." + ExtensionFile;
                    var path = Path.Combine(Server.MapPath("~/public/images/product"), namefilenew);
                    var folder = Server.MapPath("~/public/images/product/" + namecate);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    file.SaveAs(path);
                    book.img = namefilenew;
                }
                book.slug = slug;
                book.date_updated = DateTime.Now;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                Message.set_flash("Sửa thành công", "success");
                ViewBag.listCate = db.Categories.Where(m => m.status != 0 && m.ID > 2).ToList();
                return RedirectToAction("Index");
            }
            ViewBag.listCate = db.Categories.Where(m => m.status != 0 && m.ID > 2).ToList();
            ViewBag.catid = new SelectList(db.Categories, "ID", "name", book.catid);
            ViewBag.publishid = new SelectList(db.Publishers, "ID", "name", book.publishid);
            return View(book);
        }

        public ActionResult Status(int id)
        {
            Book book = db.Books.Find(id);
            book.status = (book.status == 1) ? 2 : 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult trash()
        {
            var list = db.Books.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            Book book = db.Books.Find(id);
            book.status = 0;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Retrash(int id)
        {
            Book book = db.Books.Find(id);
            book.status = 2;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("trash");
        }

        public ActionResult deleteTrash(int id)
        {
            Book mbook = db.Books.Find(id);
            db.Books.Remove(mbook);
            db.SaveChanges();
            Message.set_flash("Đã xóa vĩnh viễn 1 sản phẩm", "success");
            return RedirectToAction("trash");
        }
    }
}
