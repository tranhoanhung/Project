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
        //Ok hết nha!!!
        BookshopEntities db = new BookshopEntities();
        // GET: Favorite
        public ActionResult Index()
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            var list = db.Favorites.Where(x => x.userid == sessionUser.ID).ToList();
            return View("favoriteList", list);
            //ok
        }

        //List favorite right
        public ActionResult Heart()
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.ID = sessionUser.ID;
                ViewBag.total = db.Favorites.Where(m => m.userid == sessionUser.ID).Count();
            }  
            return View("loveRight");
        }

        //Thêm vào danh sách -->test ok
        public ActionResult Additemlove(int bookId)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            var myFavorite = db.Users.Where(u => u.ID == sessionUser.ID).SingleOrDefault().Favorites.ToList();
            //kiểm tra list fv của user này đã có bookid này chưa
            if (myFavorite.Any(b => b.bookid == bookId))
            {
                Message.set_flash("Sản phẩm đã có trong danh mục yêu thích", "danger");
                return Redirect("~/yeu-thich/" + sessionUser.ID);
            }
            //nếu chưa có
            else
            {
                var item = new Favorite();
                item.bookid = bookId;
                item.userid = sessionUser.ID;
                db.Favorites.Add(item);
                db.SaveChanges();
                Message.set_flash("Thêm thành công", "success");
                return Redirect("~/yeu-thich/" + sessionUser.ID);
            }
        }

        //Xóa item -->test ok
        public ActionResult detete(int id)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            Favorite favorite = db.Favorites.Find(id);
            db.Favorites.Remove(favorite);
            db.SaveChanges();
            Message.set_flash("Đã xóa sách yêu thích", "success");
            return Redirect("~/yeu-thich/" + sessionUser.ID);
        }

    }
}