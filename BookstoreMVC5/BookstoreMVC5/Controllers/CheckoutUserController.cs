﻿using BookstoreMVC5.library;
using BookstoreMVC5.Models;
using BookstoreMVC5.MomoAPI;
using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookstoreMVC5.Controllers
{
    public class CheckoutUserController : Controller
    {
        private const string SessionCart = "SessionCart";
        BookshopEntities db = new BookshopEntities();
        // GET: Checkout
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index(int? id)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            var list = db.CartUsers.Where(x => x.userid == sessionUser.ID).ToList();
            if (list == null)
            {


                ViewBag.statusCart = "Danh sách trống";
            }
            return View(list);
        }


        //ok thanh toán
        [HttpPost]
        public ActionResult Index(Order order, int? id)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int numIterations = 0;
            numIterations = rand.Next(1, 100000);
            DateTime time = DateTime.Now;
            string orderCode = Mystring.ToStringNospace(numIterations + "" + time);
            string sumOrder = Request["sumOrder"];
            string payment_method = Request["option_payment"];
            // Neu Ship COde
            if (payment_method.Equals("COD"))
            {
                // cap nhat thong tin sau khi dat hang thanh cong
                saveOrder(order, "COD", 2, orderCode);
                var list = db.CartUsers.Where(x => x.userid == id).ToList();
                var listProductOrder = db.Orders.Where(m => m.ID == order.ID).FirstOrDefault();
                return View("oderComplete", listProductOrder);
            }
            //Neu Thanh toan MOMO
            else if (payment_method.Equals("MOMO"))
            {
                //request params need to request to MoMo system
                string endpoint = momoInfo.endpoint;
                string partnerCode = momoInfo.partnerCode;
                string accessKey = momoInfo.accessKey;
                string serectkey = momoInfo.serectkey;
                string orderInfo = momoInfo.orderInfo;
                string returnUrl = momoInfo.returnUrl;
                string notifyurl = momoInfo.notifyurl;

                string amount = sumOrder;
                string orderid = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                log.Debug("rawHash = " + rawHash);

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);
                log.Debug("Signature = " + signature);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
                log.Debug("Json request to MoMo: " + message.ToString());
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);

                saveOrder(order, "Cổng thanh toán MOMO", 2, orderid);
                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
            return View("payment");
        }
        //end



        //Khi huy thanh toán MOMO
        public ActionResult cancel_order_momo()
        {
            return View("cancel_order");
        }
        //Khi Thanh toám momo xong
        public ActionResult confirm_orderPaymentOnline_momo()
        {
            
            String errorCode = Request["errorCode"];
            String orderId = Request["orderId"];
            if (errorCode == "0")
            {

                var OrderInfo = db.Orders.Where(m => m.code == orderId).FirstOrDefault();
                db.Entry(OrderInfo).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Methodpayment = OrderInfo.deliveryPaymentMethod;
                return View("oderComplete", OrderInfo);
            }
            else
            {
                ViewBag.status = false;
            }

            return View("confirm_orderPaymentOnline");
        }
        //function ssave order when order success!
        public void saveOrder(Order order, string paymentMethod, int StatusPayment, string ordercode)
        {
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            var list = db.CartUsers.Where(x => x.userid == sessionUser.ID).ToList();                   
            if (ModelState.IsValid)
            {
                order.code = ordercode;
                order.userid = sessionUser.ID;
                order.deliveryPaymentMethod = paymentMethod;
                order.date_created = DateTime.Now;
                order.status = 2;
                order.deliverydate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                Session["OrderId"] = ordercode;
                ViewBag.name = order.deliveryname;
                ViewBag.email = order.deliveryemail;
                ViewBag.address = order.deliveryaddress;
                ViewBag.code = order.code;
                ViewBag.phone = order.deliveryphone;


                foreach (var item in list)
                {
                    Orderdetail orderdetail = new Orderdetail();
                    float price = 0;
                    int sale = (int)item.Book.pricesale;
                    if (sale > 0)
                    {
                        price = ((int)item.Book.price - (int)item.Book.price / 100 * (int)sale) * ((int)item.quantity);
                    }
                    else
                    {
                        price = (float)item.Book.price * (int)item.quantity;
                    }
                    orderdetail.orderid = order.ID;
                    orderdetail.bookid = item.Book.ID;
                    orderdetail.userid = sessionUser.ID;
                    //orderdetail.pricesale = (int)item.Book.pricesale;
                    //orderdetail.price = item.Book.price;
                    orderdetail.quantity = (int)item.quantity;
                    orderdetail.amount = price;
                    db.Orderdetails.Add(orderdetail);
                    db.SaveChanges();


                }

            }

            if (sessionUser != null)
            {
                var all = db.CartUsers.Where(x => x.userid == sessionUser.ID);
                db.CartUsers.RemoveRange(all);
                db.SaveChanges();              
            }

        }
        //

        public ActionResult productDetailCheckOut(int orderId)
        {
            var list = db.Orderdetails.Where(m => m.orderid == orderId).ToList();
            return View("_productDetailCheckOut", list);

        }
        public ActionResult orderDetail(int orderId)
        {

            var single = db.Orders.Find(orderId);
            return View("oderComplete", single);
        }

        public ActionResult subnameProduct(int id)
        {
            var list = db.Books.Find(id);
            return View("_subproductOrdersuccess", list);

        }
        public ActionResult formCheckOut()
        {
            ViewBag.username = null;
            User sessionUser = (User)Session[Sessions.CUSTOMER_SESSION];
            if (sessionUser != null)
            {
                ViewBag.username = sessionUser.username;
                ViewBag.ID = sessionUser.ID;
                ViewBag.fullname = sessionUser.fullname;
            }
            return View("_formCheckout");

        }
    }
}