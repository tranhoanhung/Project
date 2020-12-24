using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.Controllers
{
    public class ContactsController : Controller
    {
        BookshopEntities db = new BookshopEntities();
        // GET: Contacts
        public ActionResult contact()
        {
            return View("contact");
        }
        [HttpPost]
        public ActionResult contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.date_created = DateTime.Now;
                contact.status = 1;
                db.Contacts.Add(contact);
                db.SaveChanges();
                Message.set_flash("Gửi liên hệ thành công", "success");
                return View("contact");
            }
            Message.set_flash("Gửi liên hệ thất bại", "danger");
            return View("contact");
        }
    }
}