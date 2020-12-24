using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using PagedList;

namespace BookstoreMVC5.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        BookshopEntities db = new BookshopEntities();
        public ActionResult Index(String slug = "")
        {
            int page = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }
            if (slug == "")
            {
                return this.Home();
            }
            else
            {
                return this.page404();

            }
            //return View();

        }

        public ActionResult Home()
        {
            return View("Home");
        }

        public ActionResult ProductDealOfDay()
        {
            var ProductDealOfDay = db.Books.Where(m => m.status == 1 && m.pricesale != 0).OrderByDescending(m => m.pricesale).Take(10).ToList();
            ViewBag.category = db.Categories.Where(m => m.status == 1).ToList();
            var kmCaonhat = db.Books.Where(m => m.status == 1 && m.pricesale != 0).OrderByDescending(m => m.pricesale).First();
            ViewBag.discout = kmCaonhat.pricesale;
            return View("Product_Deal_Of_Day", ProductDealOfDay);
        }

        public ActionResult ProductDetail(string slug)
        {
            var SingleProduct = db.Books.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.category = db.Categories.Find(SingleProduct.catid);
            ViewBag.publisher = db.Publishers.Single(m => m.ID == SingleProduct.publishid).name;
            return View("productDetail", SingleProduct);
        }

        public ActionResult reladProduct(int catId)
        {
            var list = db.Books.Where(m => m.status == 1 && m.catid == catId).Take(5).ToList();
            var SingleProduct = db.Books.Where(m => m.status == 1).First();
            ViewBag.category = db.Categories.Single(m => m.ID == SingleProduct.catid).name;
            ViewBag.publisher = db.Publishers.Single(m => m.ID == SingleProduct.publishid).name;
            return View("reladProductCart", list);
        }
        public ActionResult buyWithProduct(int catId, int productId)
        {
            var list = db.Books.Where(m => m.status == 1 && m.catid == catId)
                .Where(m => m.ID != productId)
                .OrderBy(m => Guid.NewGuid())
                .Take(2)
                .ToList();
            return View("buyWithProduct", list);
        }
        public ActionResult productOfCategory(string slug, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            Category single = db.Categories.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.url = single.slug + "";
            ViewBag.blace = single.name;
            ViewBag.category = db.Categories.Where(m => m.status == 1).ToList();
            var list = db.Books
                .Where(m => m.status == 1 && m.catid == single.ID)
                 .ToList();
            return View("allProduct", list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult allProduct(int? page)
        {
            ViewBag.url = "san-pham";
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.category = db.Categories.Where(m => m.status == 1);
            var list = db.Books.Where(m => m.status == 1).ToList().ToPagedList(pageNumber, pageSize);
            ViewBag.allItem = db.Books.Where(m => m.status == 1).Count();
            return View("allProduct", list);
        }

        public ActionResult ProductNew()
        {
            var list = db.Books.Where(m => m.status == 1).OrderByDescending(m => m.ID).Take(10)
                .ToList();
            return View("reladProduct", list);
        }
        public ActionResult Bestseller()
        {
            var list = db.Books.Where(m => m.status == 1).OrderBy(m => m.pricesale).Take(3)
                .ToList();
            return View("reladProduct", list);
        }
        public ActionResult BestsellerCart()
        {
            var list = db.Books.Where(m => m.status == 1).OrderBy(m => m.pricesale).Take(3)
                .ToList();
            return View("reladProductCart", list);
        }
        public ActionResult FlashSale()
        {
            ViewBag.category = db.Categories.Where(m => m.status == 1).ToList();
            var list = db.Books.Where(m => m.status == 1).OrderByDescending(m => m.pricesale).Take(5)
                .ToList();
            return View("FlashSale", list);
        }
        public ActionResult SearchProduct(string keyw, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.url = "tim-kiem/?keyw=" + keyw + "";
            @ViewBag.blace = keyw;
            ViewBag.category = db.Categories.Where(m => m.status == 1).ToList();
            var list = db.Books.Where(m => m.status == 1 && m.name.Contains(keyw)).OrderBy(m => m.ID);
            return View("allProduct", list.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult homeCategory()
        {
            var list = db.Categories.Where(m => m.status == 1).OrderBy(m => m.ID).Take(3)
                .ToList();
            return View("HomeBookOfCategory", list);
        }
        public ActionResult homeBookOfCategory(int catId)
        {
            var list = db.Books.Where(m => m.status == 1 && m.catid == catId).OrderByDescending(m => m.pricesale).Take(5)
                .ToList();
            return View("reladProduct", list);
        }


        public ActionResult page404()
        {
            return View("page404");
        }
   
    }
}