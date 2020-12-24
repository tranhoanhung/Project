using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using BookstoreMVC5.library;

namespace BookstoreMVC5.Controllers
{
    public class FavoriteController : Controller
    {
        BookshopEntities db = new BookshopEntities();
        // GET: Favorite
        public ActionResult Index(int? id)
        {
            var list = db.Favorites.Where(x => x.userid == id).ToList();
            if(list == null)
            {
                ViewBag.statusCart = "Danh sách trống";
            }
            return View("favoriteList", list);
        }

        public ActionResult Additemlove(int? id, int bookId)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            var item = new Favorite();
            var list = db.Favorites.Where(x => x.userid == id).ToList();
            Book book = db.Books.Find(bookId);
            //chưa có
            if(sessionUser == null)
            {
                Response.Redirect("~/login-logout");
            }
            else
            {
                //item.bookid = bookId;
                //item.userid = sessionUser.ID;
                //db.Favorites.Add(item);
                //db.SaveChanges();
                //Message.set_flash("Thêm thành công", "success");
                //return Redirect("~/yeu-thich/" + sessionUser.ID);
                if ((book.ID != bookId) || (sessionUser.ID != id) )
                {
                    item.bookid = bookId;
                    item.userid = sessionUser.ID;
                    db.Favorites.Add(item);
                    db.SaveChanges();
                    Message.set_flash("Thêm thành công", "success");
                    return Redirect("~/yeu-thich/" + sessionUser.ID);
                }
                else
                {
                    Message.set_flash("Sản phẩm đã có trong danh mục yêu thích", "danger");
                    return Redirect("~/yeu-thich/" + sessionUser.ID);
                }
                
            }
            return RedirectToAction("Index");
        }
    }
}