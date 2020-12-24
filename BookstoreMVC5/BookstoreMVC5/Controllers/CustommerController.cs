using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using System.Net;
using BookstoreMVC5.library;
using System.Threading.Tasks;

namespace BookstoreMVC5.Controllers
{
    public class CustommerController : Controller
    {
        // GET: Customer
        BookshopEntities db = new BookshopEntities();
        public ActionResult Index(int? id)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            return View(sessionUser);
        }
        public ActionResult logout()
        {
            Message.set_flash("Đăng xuất thành công", "success");
            Session[Sessions.CUSTOMER_SESSION] = null;
            Session["SessionCart"] = null;
            Response.Redirect("~/login-logout");
            return View();
        }
        public ActionResult formEditCustomer()
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            return View("_formEditCustomer", sessionUser);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                user.status = 1;
                user.img = "defalt.png";
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session.Add(Sessions.CUSTOMER_SESSION, user);
                Message.set_flash("Cập nhật thành công", "success");
                Response.Redirect("~/thong-tin-kh" );

            }
            return View(user);
        }


        public ActionResult ChangePassWord(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("_changePassword", user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassWord(User muser, FormCollection fc)
        {
            string oldPass = Mystring.ToMD5(fc["passOld"]);
            string rePass = Mystring.ToMD5(fc["rePass"]);
            string newPass = Mystring.ToMD5(fc["password1"]);
            var pass_account = db.Users.Where(m => m.password == oldPass).ToList().Count();
            if (pass_account == 0)
            {
                ViewBag.status = "Mật khẩu cũ không đúng";
                return View("_changePassword", muser);
            }
            else if (rePass != newPass)
            {
                ViewBag.status = "2 Mật khẩu không khớp";
                return View("_changePassword", muser);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var updatedPass = db.Users.Find(muser.ID);

                    updatedPass.fullname = muser.fullname;
                    updatedPass.username = muser.username;
                    updatedPass.email = muser.email;
                    updatedPass.phone = muser.phone;
                    updatedPass.gender = muser.gender;
                    updatedPass.img = "bav";
                    updatedPass.password = newPass;
                    updatedPass.status = 1;

                    db.Users.Attach(updatedPass);
                    db.Entry(updatedPass).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                    Message.set_flash("Đổi mật khẩu thành công", "success");
                    //Response.Redirect("~/thong-tin-kh");
                    return Redirect("~/thong-tin-kh/");
                }
            }
            return View("_changePassword", muser);
        }


    }
}
