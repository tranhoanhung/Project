using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.library;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.Controllers
{
    public class CartUserController : Controller
    {
        BookshopEntities db = new BookshopEntities();
        // GET: CartUser
        public ActionResult Index()
        {                     
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            var list = db.CartUsers.Where(x => x.userid == sessionUser.ID).ToList();
            return View("cartList", list);
        }
        public ActionResult CartRight()
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.ID = sessionUser.ID;
                ViewBag.total = db.CartUsers.Where(m => m.userid == sessionUser.ID).Count();
            }

            return View("cartRight");
        }

        public ActionResult cart_header()
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.total = db.CartUsers.Where(m => m.userid == sessionUser.ID).Count();
                ViewBag.listCart = db.CartUsers.Where(m => m.userid == sessionUser.ID).ToList();
            }
            return View("_cartHeader");
        }

        public JsonResult Additembook(int userID, long productID, int quantity)
        {
            var item = new CartItem();
            var findProduct = db.Books.Find(productID);
            var cart = db.CartUsers.Count();
            if (cart > 0)
            {
                var list = db.CartUsers.ToList();
                var myCart = db.Users.Where(u => u.ID == userID).SingleOrDefault().CartUsers.ToList();
                if (myCart.Any(m => m.bookid == productID))
                {
                    int quantity1 = 0;
                    bool bad = false;
                    foreach (var item1 in list)
                    {
                        if (item1.bookid == productID)
                        {
                            if ((item1.quantity + quantity) > findProduct.number)
                            {
                                bad = true;
                            }
                            else
                            {
                                item1.quantity += quantity;
                                db.Entry(item1).State = EntityState.Modified;
                                db.SaveChanges();
                                quantity1 = (int)item1.quantity;
                            }
                        }
                    }
                    return Json(item, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    //kiểm tra có tồn tại k
                    item.meThod = "cartExist";
                    item.f = false;
                    if (quantity > findProduct.number)
                    {
                        item.f = true;
                        item.quantity = 0;
                    }
                    else
                    {
                        item.quantity = quantity;
                        CartUser cartforUser = new CartUser();
                        cartforUser.userid = userID;
                        cartforUser.bookid = (int)productID;
                        cartforUser.quantity = quantity;
                        db.CartUsers.Add(cartforUser);
                        db.SaveChanges();
                        item.meThod = "cartExist";
                    }
                    return Json(item, JsonRequestBehavior.AllowGet);
                }
            }
            //chưa có thì tạo cartforuser add vào
            else
            {
                if (quantity > findProduct.number)
                {
                    item.f = true;
                    item.quantity = 0;
                }
                else
                {
                    item.quantity = quantity;
                    CartUser cartforUser = new CartUser();
                    cartforUser.userid = userID;
                    cartforUser.bookid = (int)productID;
                    cartforUser.quantity = quantity;
                    db.CartUsers.Add(cartforUser);
                    db.SaveChanges();
                }
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult detete(int id)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            CartUser cartUser = db.CartUsers.Find(id);
            db.CartUsers.Remove(cartUser);
            db.SaveChanges();
            Message.set_flash("Đã xóa sản phẩm", "success");
            return Redirect("~/gio-hang-user/" + sessionUser.ID);
        }

    }
}