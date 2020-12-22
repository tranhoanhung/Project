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
            return View("BookOfCategory", list);
        }
        public ActionResult bookOfCategory(int catId)
        {
            ViewBag.category = db.Categories.Where(m => m.status == 1).ToList();
            var list = db.Books.Where(m => m.status == 1 && m.catid == catId).OrderByDescending(m => m.pricesale).Take(10)
                .ToList();
            return View("readProduct", list);
        }




        //     public JsonResult GetQuickView(int id)
        //     {
        //         ImageProducts image = new ImageProducts();
        //         var result = (from p in db.Products
        //                       join c in db.Categorys
        //on p.CategoryID equals c.CategoryID
        //                       where p.ProductID == id && p.Status == true
        //                       select new ProductModel
        //                       {
        //                           ProductID = p.ProductID,
        //                           ProductName = p.ProductName,
        //                           Price = p.Price,
        //                           PriceSale = p.PriceSale,
        //                           Alias = p.Alias,
        //                           Images = p.Images,
        //                           Quanlity = p.Quanlity,
        //                           CategoryID = p.CategoryID,
        //                           Status = p.Status,
        //                           DescriptShort = p.DescriptShort,
        //                           CategoryName = c.CategoryName,
        //                       }).ToList();

        //         image.Product = result;
        //         var image_product = (from i in db.ImageProduct
        //                              join p in db.Products
        //                              on i.ProductID equals p.ProductID
        //                              where p.ProductID == id && p.Status == true
        //                              select new ImageMores
        //                              {
        //                                  FileImages = i.FileImages
        //                              }).ToList();
        //         image.Image = image_product;
        //         return new JsonResult() { Data = image, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
    }
}