using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongCTSV
{
    public class PhongCTSVAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PhongCTSV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PhongCTSV_default",
                "PhongCTSV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}