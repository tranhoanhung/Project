using BookstoreMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BookstoreMVC5.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private const string SessionCart = "SessionCart";

        BookshopEntities db = new BookshopEntities();
        public ActionResult Index()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            else if (cart == null)
            {
                ViewBag.statusCart = "Giỏ hàng trống";

            }
            return View(list);
        }


        public ActionResult AddItem(long bookid, int quantity)
        {
            var cart = Session[SessionCart];
            Book book = db.Books.Find(bookid);
            //if kiểm trả nếu đã có rồi
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                //nếu chứa
                int quantity1 = 0;
                if (list.Exists(m => m.book.ID == bookid))
                {
                    //+ số lượng
                    foreach (var item1 in list)
                    {
                        if (item1.book.ID == bookid)
                        {
                            item1.quantity += quantity;
                            quantity1 = item1.quantity;
                        }
                    }
                    //+ giá tiền
                    int pricetotal = 0;
                    foreach (var item in list)
                    {
                        if (item.book.pricesale > 0)
                        {
                            int temp = (((int)item.book.price) - ((int)item.book.price / 100 * (int)item.book.pricesale))
                                * ((int)item.quantity);

                            pricetotal += temp;
                        }
                        else
                        {
                            int temp = (int)item.book.price * (int)item.quantity;
                            pricetotal += temp;
                        }
                    }
                    //end


                }
                else
                {
                    var item = new CartItem();
                    item.book = book;
                    item.quantity = quantity;
                    list.Add(item);
                    //gắn vào session

                }
                Session[SessionCart] = list;
            }
            else
            {
                //chưa có gì
                //tạo đối tượng cart item
                var item = new CartItem();
                item.book = book;
                item.quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //gán vào session
                Session[SessionCart] = list;

            }
            return RedirectToAction("Index");
        }
        
        public JsonResult deleteitem(long bookid)
        {
            var item = new CartItem();
            var cart = Session[SessionCart];
            var list = (List<CartItem>)cart;
            CartItem itemXoa = list.FirstOrDefault(m => m.book.ID == bookid);
            //kiểm tra 
            if (itemXoa != null)
            {
                list.Remove(itemXoa);
                if (list.Count == 0)
                {
                    Session["SessionCart"] = null;
                }
            }

            item.countCart = list.Count();
            int priceTotal = 0;
            foreach (var item1 in list)
            {
                if (item1.book.pricesale > 0)
                {
                    int temp = (((int)item1.book.price) - ((int)item1.book.price / 100 * (int)item1.book.pricesale)) * ((int)item1.quantity);
                    priceTotal += temp;
                }
                else
                {
                    int temp = (int)item1.book.price * (int)item1.quantity;
                    priceTotal += temp;
                }
            }
            item.priceTotal = priceTotal;
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        


        public ActionResult cart_header()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            float priceTotol = 0;
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                foreach (var item1 in list)
                {
                    if (item1.book.pricesale > 0)
                    {
                        int temp = (((int)item1.book.price) - ((int)item1.book.price / 100 * (int)item1.book.pricesale)) * ((int)item1.quantity);

                        priceTotol += temp;
                    }
                    else
                    {
                        int temp = (int)item1.book.price * (int)item1.quantity;
                        priceTotol += temp;
                    }
                }
            }
            ViewBag.CartTotal = priceTotol;
            return View("_cartHeader", list);
        }

        //public RedirectToRouteResult updateitem(long P_SanPhamID, int P_quantity)
        //{
        //    var cart = Session[SessionCart];
        //    var list = (List<CartItem>)cart;
        //    CartItem itemSua = list.FirstOrDefault(m => m.book.ID == P_SanPhamID);
        //    if (itemSua != null)
        //    {
                
        //        itemSua.quantity = P_quantity;
        //    }
        //    return RedirectToAction("Index");
        //}
        //public JsonResult updateitem(long bookid, int quantity)
        //{
        //    var item = new CartItem();
        //    var cart = Session[SessionCart];
        //    var list = (List<CartItem>)cart;
        //    CartItem itemSua = list.FirstOrDefault(m => m.book.ID == bookid);
        //    //kiểm tra 
        //    if (itemSua != null)
        //    {
        //        if (itemSua.quantity + quantity > itemSua.book.number)
        //        {
        //            ViewBag.statusCart = "số lượng quá nhiều";
        //        }
        //        itemSua.quantity = quantity;
        //    }
        //    return Json(item, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[SessionCart];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.book.ID == item.book.ID);
                if (jsonItem != null)
                {
                    item.quantity = jsonItem.quantity;
                }
            }
            Session[SessionCart] = sessionCart;
            return Json(new
            {
                status = true
            });
        }


        //public RedirectToRouteResult deleteitem(long productID)
        //{
        //    var cart = Session[SessionCart];
        //    var list = (List<Cart_item>)cart;

        //    Cart_item itemXoa = list.FirstOrDefault(m => m.product.ID == productID);
        //    if (itemXoa != null)
        //    {
        //        list.Remove(itemXoa);
        //        if (list.Count == 0)
        //        {
        //            Session["SessionCart"] = null;
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}



    }
}