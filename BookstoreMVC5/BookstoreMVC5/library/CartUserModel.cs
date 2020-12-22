using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookstoreMVC5.Models;

namespace BookstoreMVC5
{
    public class CartUserModel
    {
        BookshopEntities db = new BookshopEntities();
        public static void AddUserCart(CartUser cartUser, int bookiD, int useriD)
        {
            
            cartUser.bookid = bookiD;
            cartUser.userid = useriD;
        }
    }
}