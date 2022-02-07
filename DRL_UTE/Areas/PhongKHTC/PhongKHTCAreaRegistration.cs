using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongKHTC
{
    public class PhongKHTCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PhongKHTC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PhongKHTC_default",
                "PhongKHTC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}