﻿using System;
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
    public class SlidersController : Controller
    {
        private BookshopEntities db = new BookshopEntities();

        // GET: Admin/Sliders
        public ActionResult Index()
        {
            var list = db.Sliders.Where(m => m.status != 0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        // GET: Admin/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        public ActionResult Create()
        {
            ViewBag.listCate = db.Sliders.Where(m => m.status != 0).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                file = Request.Files["img"];
                string filename = file.FileName.ToString();
                string name = Mystring.ToSlug(slider.name.ToString());
                string ExtensionFile = Mystring.GetFileExtension(filename);
                string namefilenew = name + "." + ExtensionFile;
                var path = Path.Combine(Server.MapPath("~/Public/images/bg-images"), namefilenew);

                file.SaveAs(path);
                slider.img = namefilenew;
                slider.date_created = DateTime.Now;
                db.Sliders.Add(slider);
                db.SaveChanges();
                Message.set_flash("Thêm thành công", "success");
                return RedirectToAction("Index");
            }
            Message.set_flash("Thêm thất bại", "danger");
            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.listCate = db.Sliders.Where(m => m.status != 0).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string name = Mystring.ToSlug(slider.name.ToString());
                HttpPostedFileBase file = Request.Files["img"];
                string filename = file.FileName.ToString();
                if (filename.Equals("") == false)
                {
                    string ExtensionFile = Mystring.GetFileExtension(filename);
                    string namefilenew = name + "." + ExtensionFile;
                    var path = Path.Combine(Server.MapPath("~/Public/images/bg-images"), namefilenew);
                    file.SaveAs(path);
                    slider.img = namefilenew;
                }
                slider.date_created = DateTime.Now;
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listCate = db.Sliders.Where(m => m.status != 0).ToList();
            return View(slider);
        }

        public ActionResult Status(int id)
        {
            Slider slider = db.Sliders.Find(id);
            slider.status = (slider.status == 1) ? 2 : 1;
            db.Entry(slider).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Thay đổi trang thái thành công", "success");
            return RedirectToAction("Index");
        }

        public ActionResult trash()
        {
            var list = db.Sliders.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            Slider slider = db.Sliders.Find(id);
            slider.status = 0;
            db.Entry(slider).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Retrash(int id)
        {
            Slider slider = db.Sliders.Find(id);
            slider.status = 2;
            db.Entry(slider).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("trash");
        }
        public ActionResult deleteTrash(int id)
        {
            Slider slider = db.Sliders.Find(id);
            db.Sliders.Remove(slider);
            db.SaveChanges();
            Message.set_flash("Đã xóa vĩnh viễn 1 sản phẩm", "success");
            return RedirectToAction("trash");
        }
    }
}
