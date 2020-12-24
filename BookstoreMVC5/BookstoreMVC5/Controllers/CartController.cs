using BookstoreMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BookstoreMVC5.library;

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
                    if (item1.product.pricesale > 0)
                    {
                        int temp = (((int)item1.product.price) - ((int)item1.product.price / 100 * (int)item1.product.pricesale)) * ((int)item1.quantity);

                        priceTotol += temp;
                    }
                    else
                    {
                        int temp = (int)item1.product.price * (int)item1.quantity;
                        priceTotol += temp;
                    }
                }
            }
            ViewBag.CartTotal = priceTotol;
            return View("_cartHeader", list);
        }

        public JsonResult Additem(long productID, int quantity)
        {
            var item = new CartItem();
            Book product = db.Books.Find(productID);
            var cart = Session[SessionCart];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(m => m.product.ID == productID))
                {
                    int quantity1 = 0;
                    bool bad = false;
                    foreach (var item1 in list)
                    {
                        if (item1.product.ID == productID)
                        {
                            if ((item1.quantity + quantity) > item1.product.number)
                            {
                                bad = true;

                            }
                            else
                            {
                                item1.quantity += quantity;
                                quantity1 = item1.quantity;
                            }
                        }
                    }
                    int priceTotol = 0;
                    foreach (var item1 in list)
                    {
                        if (item1.product.pricesale > 0)
                        {
                            int temp = (((int)item1.product.price) - ((int)item1.product.price / 100 * (int)item1.product.pricesale))
                                * ((int)item1.quantity);

                            priceTotol += temp;
                        }
                        else
                        {
                            int temp = (int)item1.product.price * (int)item1.quantity;
                            priceTotol += temp;
                        }

                    }
                    return Json(new
                    {
                        ProductPrice = ((int)product.price) - (((int)product.price / 100) * ((int)product.pricesale)),
                        bad = bad,
                        price = product.price,
                        priceSale = product.pricesale,
                        quantity = quantity1,
                        priceTotol = priceTotol,
                        productID = productID,
                        meThod = "updateQuantity"
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    item.meThod = "cartExist";
                    item.f = false;
                    if (quantity > product.number)
                    {
                        item.f = true;
                        item.quantity = 0;
                    }
                    else
                    {
                        item.quantity = quantity;
                        list.Add(item);
                        item.product = product;
                        item.countCart = list.Count();
                        item.meThod = "cartExist";
                        int priceTotol = 0;
                        foreach (var item1 in list)
                        {
                            if (item1.product.pricesale > 0)
                            {
                                int temp = (((int)item1.product.price) - ((int)item1.product.price / 100 * (int)item1.product.pricesale)) * ((int)item1.quantity);
                                priceTotol += temp;
                            }
                            else
                            {
                                int temp = (int)item1.product.price * (int)item1.quantity;
                                priceTotol += temp;
                            }
                        }
                        item.priceTotal = priceTotol;
                        item.priceSaleee = (int)product.price - (int)product.price / 100 * (int)product.pricesale;
                    }
                    return Json(item, JsonRequestBehavior.AllowGet);
                }
            }
            //chưa có
            else
            {
                item.product = product;
                item.meThod = "cartEmpty";
                item.countCart = 1;
                item.f = false;
                if (quantity > product.number)
                {
                    item.f = true;
                    item.quantity = 0;
                }
                else
                {
                    item.priceSaleee = (int)product.price - (int)product.price / 100 * (int)product.pricesale;
                    item.quantity = quantity;
                    var list = new List<CartItem>();
                    list.Add(item);
                    Session[SessionCart] = list;
                    if (item.product.pricesale > 0)
                    {
                        item.priceTotal = (((int)item.product.price) - ((int)item.product.price / 100 * (int)item.product.pricesale)) * ((int)item.quantity);
                    }
                    else
                    {
                        item.priceTotal = (int)product.price;
                    }
                    item.priceTotal = (((int)item.product.price) - ((int)item.product.price / 100 * (int)item.product.pricesale)) * ((int)item.quantity);
                }
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public RedirectToRouteResult deleteitem(long productID)
        {
            var cart = Session[SessionCart];
            var list = (List<CartItem>)cart;

            CartItem itemXoa = list.FirstOrDefault(m => m.product.ID == productID);
            if (itemXoa != null)
            {
                list.Remove(itemXoa);
                if (list.Count == 0)
                {
                    Session["SessionCart"] = null;
                }
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult updateitem(long P_SanPhamID, int P_quantity)
        {
            var cart = Session[SessionCart];
            var list = (List<CartItem>)cart;
            CartItem itemSua = list.FirstOrDefault(m => m.product.ID == P_SanPhamID);
            if (itemSua != null)
            {
                if (itemSua.quantity + P_quantity > itemSua.product.number)
                {
                    Message.set_flash("Số lượng bạn chọn đã lớn hơn số lượng trong kho", "danger");
                    return RedirectToAction("Index");
                }
                itemSua.quantity = P_quantity;
            }
            return RedirectToAction("Index");
        }


        //public RedirectToRouteResult updateitem(long P_SanPhamID, int P_quantity)
        //{
        //    var cart = Session[SessionCart];
        //    var list = (List<CartItem>)cart;
        //    CartItem itemSua = list.FirstOrDefault(m => m.product.ID == P_SanPhamID);
        //    if (itemSua != null)
        //    {
        //        itemSua.quantity = P_quantity;
        //    }
        //    return RedirectToAction("Index");
        //}

        //public RedirectToRouteResult deleteitem(long productID)
        //{
        //    var cart = Session[SessionCart];
        //    var list = (List<CartItem>)cart;

        //    CartItem itemXoa = list.FirstOrDefault(m => m.product.ID == productID);
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



        //public ActionResult AddItem(int bookid, int quantity)
        //{
        //    User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
        //    var cart = Session[SessionCart];
        //    CartUser cartUser = new CartUser();
        //    Book book = db.Books.Find(bookid);
        //    //if kiểm trả nếu đã có rồi
        //    if (cart != null)
        //    {
        //        var list = (List<CartItem>)cart;
        //        //nếu chứa
        //        int quantity1 = 0;
        //        if (list.Exists(m => m.book.ID == bookid))
        //        {
        //            //+ số lượng
        //            foreach (var item1 in list)
        //            {
        //                if (item1.book.ID == bookid)
        //                {
        //                    item1.quantity += quantity;
        //                    quantity1 = item1.quantity;
        //                }
        //            }
        //            //+ giá tiền
        //            int pricetotal = 0;
        //            foreach (var item in list)
        //            {
        //                if (item.book.pricesale > 0)
        //                {
        //                    int temp = (((int)item.book.price) - ((int)item.book.price / 100 * (int)item.book.pricesale))
        //                        * ((int)item.quantity);

        //                    pricetotal += temp;
        //                }
        //                else
        //                {
        //                    int temp = (int)item.book.price * (int)item.quantity;
        //                    pricetotal += temp;
        //                }
        //            }
        //            //end


        //        }
        //        else
        //        {
        //            var item = new CartItem();
        //            item.book = book;
        //            item.quantity = quantity;
        //            list.Add(item);



        //            //gắn vào session

        //        }
        //        Session[SessionCart] = list;

        //        //cartUser.bookid = bookid;
        //        //cartUser.quantity = quantity;
        //        //cartUser.userid = sessionUser.ID;
        //        //db.CartUsers.Add(cartUser);
        //    }
        //    else
        //    {

        //        //chưa có gì
        //        //tạo đối tượng cart item
        //        var item = new CartItem();
        //        item.book = book;
        //        item.quantity = quantity;
        //        var list = new List<CartItem>();
        //        list.Add(item);
        //        //gán vào session
        //        Session[SessionCart] = list;
        //    }
        //    return RedirectToAction("Index");
        //}

        //public JsonResult deleteitem(long bookid)
        //{
        //    var item = new CartItem();
        //    var cart = Session[SessionCart];
        //    var list = (List<CartItem>)cart;
        //    CartItem itemXoa = list.FirstOrDefault(m => m.book.ID == bookid);
        //    //kiểm tra 
        //    if (itemXoa != null)
        //    {
        //        list.Remove(itemXoa);
        //        if (list.Count == 0)
        //        {
        //            Session["SessionCart"] = null;
        //        }
        //    }

        //    item.countCart = list.Count();
        //    int priceTotal = 0;
        //    foreach (var item1 in list)
        //    {
        //        if (item1.book.pricesale > 0)
        //        {
        //            int temp = (((int)item1.book.price) - ((int)item1.book.price / 100 * (int)item1.book.pricesale)) * ((int)item1.quantity);
        //            priceTotal += temp;
        //        }
        //        else
        //        {
        //            int temp = (int)item1.book.price * (int)item1.quantity;
        //            priceTotal += temp;
        //        }
        //    }
        //    item.priceTotal = priceTotal;
        //    return Json(item, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Update(string cartModel)
        //{
        //    var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
        //    var sessionCart = (List<CartItem>)Session[SessionCart];

        //    foreach (var item in sessionCart)
        //    {
        //        var jsonItem = jsonCart.SingleOrDefault(x => x.book.ID == item.book.ID);
        //        if (jsonItem != null)
        //        {
        //            item.quantity = jsonItem.quantity;
        //        }
        //    }
        //    Session[SessionCart] = sessionCart;
        //    return Json(new
        //    {
        //        status = true
        //    });
        //}

    }
}