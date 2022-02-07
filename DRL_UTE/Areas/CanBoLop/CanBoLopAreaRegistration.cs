using System.Web.Mvc;

namespace DRL_UTE.Areas.CanBoLop
{
    public class CanBoLopAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CanBoLop";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CanBoLop_default",
                "CanBoLop/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}