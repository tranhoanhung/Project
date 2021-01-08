using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;
using BookstoreMVC5.library;
using Facebook;
using System.Configuration;

namespace BookstoreMVC5.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,email");
                string email = me.email;
                string username = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.email = email;
                user.username = email;
                user.password = Mystring.ToMD5(email);
                user.status = 1;
                user.fullname = lastname + " " + firstname;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    Session.Add(Sessions.CUSTOMER_SESSION, user);
                    if (!Response.IsRequestBeingRedirected)
                        Message.set_flash("Đăng nhập thành công", "success");
                    Response.Redirect("~/thong-tin-kh");
                }
            }
            return Redirect("/");
        }
        BookshopEntities db = new BookshopEntities();

        public ActionResult Index()
        {
            return View("login");
        }
        

        [HttpPost]
        public void login(FormCollection fc)
        {
            string Username = fc["txtusername"];
            string Pass = Mystring.ToMD5(fc["txtpassword"]);
            string PassNoMD5 = fc["txtpassword"];
            var user_account = db.Users.Where(m => m.username == Username);

            if (user_account.Count() == 0)
            {
                Message.set_flash("Tên đăng nhập không tồn tại", "danger");
                Response.Redirect("~/login-logout");
            }
            else
            {
                var pass_account = db.Users.Where(m => m.password == Pass );

                if (pass_account.Count() == 0)
                {
                    Message.set_flash("Mật khẩu không đúng", "danger");
                    Response.Redirect("~/login-logout");
                }

                else
                {
                    var user = user_account.First();
                    Session.Add(Sessions.CUSTOMER_SESSION, user);
                    if (!Response.IsRequestBeingRedirected)
                        Message.set_flash("Đăng nhập thành công", "success");
                    Response.Redirect("~/thong-tin-kh");
                }
            }
            if (!Response.IsRequestBeingRedirected)
                Response.Redirect("~/");
        }
        public void logout()
        {
            Session[Sessions.CUSTOMER_SESSION] = null;
            Response.Redirect("~/");
            Message.set_flash("Đăng xuất thành công", "success");
        }
        public ActionResult Registeruser()
        {
            return View("register");
        }

        public ActionResult register(User user, FormCollection fc)
        {
            string uname = fc["txtusername"];
            string fname = fc["txtfullname"];
            string Pass = Mystring.ToMD5(fc["txtpassword"]);
            string Pass2 = Mystring.ToMD5(fc["txtrepsassword"]);
            if (Pass2 != Pass)
            {
                ViewBag.error = "Mật khẩu không khớp";
                return View("register");
            }
            string email = fc["txtemail"];
            //string address = fc["txtaddress"];
            //string phone = fc["txtphone"];
            if (ModelState.IsValid)
            {
                var Luser = db.Users.Where(m => m.status == 1 && m.username == uname && m.status == 1);
                var Lemail = db.Users.Where(m => m.status == 1 && m.email == email && m.status == 1);
                if (Lemail.Count() > 0)
                {
                    ViewBag.erroremail = "Email đã tồn tại";
                    return View("register");
                }
                if (Luser.Count() > 0)
                {
                    ViewBag.error = "Tên Đăng Nhập đã tồn tại";
                    return View("register");
                }
                else
                {
                    user.img = "defalt.png";
                    user.password = Pass;
                    user.username = uname;
                    user.fullname = fname;
                    user.email = email;
                    //user.address = address;
                    //user.phone = phone;
                    user.status = 0;
                    db.Users.Add(user);
                    db.SaveChanges();
                    Message.set_flash("Đăng ký tài khoản thành công ", "success");
                    return View("login");
                }

            }
            Message.set_flash("Đăng ký tài khoản thất bại", "danger");
            return View("register");
        }
    }
}