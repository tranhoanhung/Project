using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using BookstoreMVC5.Common;

namespace BookstoreMVC5.Controllers
{
    public class CustommerController : Controller
    {
        // GET: Customer
        BookshopEntities db = new BookshopEntities();
        public ActionResult Index(int? id)
        {
            User sessionUser = (User)Session[Constants.CUSTOMER_SESSION];
            return View(sessionUser);
        }
        public ActionResult logout()
        {
            Message.set_flash("Đăng xuất thành công", "success");
            Session[Constants.CUSTOMER_SESSION] = null;
            Session["SessionCart"] = null;
            Response.Redirect("~/login-logout");
            return View();
        }


    }
}