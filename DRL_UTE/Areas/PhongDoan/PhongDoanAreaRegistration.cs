using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongDoan
{
    public class PhongDoanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PhongDoan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PhongDoan_default",
                "PhongDoan/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}