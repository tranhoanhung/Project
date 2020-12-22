using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookstoreMVC5.Models;

namespace BookstoreMVC5.library
{
    public class UserDao
    {
        BookshopEntities db = new BookshopEntities();
        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.username == entity.username);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }

        }
    }
}