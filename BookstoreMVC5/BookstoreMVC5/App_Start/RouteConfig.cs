using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookstoreMVC5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "yeu thich",
                url: "yeu-thich",
                defaults: new { controller = "FavoriteCustommer", action = "Additem", id = UrlParameter.Optional }
            );
            //link tất cả sản phẩm
            routes.MapRoute(
                name: "all-product",
                url: "san-pham",
                defaults: new { controller = "Site", action = "allProduct", id = UrlParameter.Optional }
            );
            //chi tiết sản phẩm
            routes.MapRoute(
                name: "productDetail",
                url: "san-pham/{slug}",
                defaults: new { controller = "Site", action = "ProductDetail", id = UrlParameter.Optional }
            );
            //sách phân theo loại
            routes.MapRoute(
                name: "productOfCategory",
                url: "loai-sach/{slug}",
                defaults: new { controller = "Site", action = "productOfCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "additem",
                url: "them-sp-gio-hang",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Gio hang",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "xoa gio hang",
                url: "xoa-gio-hang",
                defaults: new { controller = "Cart", action = "deleteitem", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "thanh toan",
                url: "thanh-toan",
                defaults: new { controller = "Checkout", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thanh toan momo",
                url: "confirm-orderPaymentOnline-momo",
                defaults: new { controller = "Checkout", action = "confirm_orderPaymentOnline_momo", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thanh toan thanh cong",
                url: "confirm-orderPaymentOnlin",
                defaults: new { controller = "Checkout", action = "confirm_orderPaymentOnline", id = UrlParameter.Optional }
            );


            //tìm kiếm
            routes.MapRoute(
                 name: "tim kiem sp",
                 url: "tim-kiem",
                 defaults: new { controller = "Site", action = "SearchProduct", id = UrlParameter.Optional }
             );
            //liên hệ
            routes.MapRoute(
                name: "lien he",
                url: "lien-he",
                defaults: new { controller = "Site", action = "contact", id = UrlParameter.Optional }
            );
            //bài viết-- hàm site-post
            routes.MapRoute(
                name: "bai viet",
                url: "bai-viet",
                defaults: new { controller = "Site", action = "allPost", id = UrlParameter.Optional }
            );
            //chi tiết bài viết -- hàm postDetaail
            routes.MapRoute(
                name: "chi tiet bai viet",
                url: "bai-viet/{slug}",
                defaults: new { controller = "Site", action = "postDetaail", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "login logout",
                url: "login-logout",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "login",
                url: "login",
                defaults: new { controller = "User", action = "login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "logout",
                url: "logout",
                defaults: new { controller = "User", action = "logout", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "register",
                url: "register",
                defaults: new { controller = "User", action = "register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thong tin khach hang",
                url: "thong-tin-kh",
                defaults: new { controller = "Custommer", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Tai khoan",
                url: "tai-khoan/{id}",
                defaults: new { controller = "Custommer", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Doi mat khau",
                url: "doi-mat-khau/{id}",
                defaults: new { controller = "Custommer", action = "ChangePassWord", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SiteSlug",
                url: "{slug}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
