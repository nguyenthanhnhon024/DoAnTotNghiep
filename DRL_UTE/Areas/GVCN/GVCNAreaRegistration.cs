using System.Web.Mvc;

namespace DRL_UTE.Areas.GVCN
{
    public class GVCNAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GVCN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GVCN_default",
                "GVCN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}