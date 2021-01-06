using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using PagedList;

namespace BookstoreMVC5.Controllers
{
    public class PostController : Controller
    {
        //ok
        BookshopEntities db = new BookshopEntities();
        // GET: Post
        public ActionResult Index ()
        {
            return View("Index");
        }

        //Hiển thị ở trang chủ
        public ActionResult postHome()
        {
            var list = db.Posts.Where(m => m.status == 1).OrderByDescending(m => m.ID).Take(3).ToList();
            return View("PostHome", list);
        }

        //Thông tin bài viết
        public ActionResult postDetails(string slug)
        {
            var SingleProduct = db.Posts.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.topic = db.Topics.Find(SingleProduct.topid);
            return View("postDetails", SingleProduct);
        }

        //Tất cả bài viết
        public ActionResult allPost(int? page)
        {
            ViewBag.url = "bai-viet";
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var list = db.Posts.Where(m => m.status == 1).OrderByDescending(m => m.ID)
                .ToList().ToPagedList(pageNumber, pageSize);
            return View("allPost", list);
        }
    }
}