using System.Web.Mvc;

namespace BookstoreMVC5.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            

            context.MapRoute(
                "Admin_books",
                "Admin/books",
                new { Controller = "Books", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Category",
                "Admin/Category",
                new { Controller = "Category", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Publisher",
                "Admin/Publisher",
                new { Controller = "Publisher", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Posts",
                "Admin/Posts",
                new { Controller = "Posts", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Topics",
                "Admin/Topics",
                new { Controller = "Topics", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_slider",
                "Admin/Sliders",
                new { Controller = "Sliders", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_contact",
                "Admin/Contact",
                new { Controller = "Contact", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { Controller = "Dashboard", action = "Index", id = UrlParameter.Optional } 
            );
        }
    }
}