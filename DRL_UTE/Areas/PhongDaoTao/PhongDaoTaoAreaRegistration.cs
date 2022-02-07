using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongDaoTao
{
    public class PhongDaoTaoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PhongDaoTao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PhongDaoTao_default",
                "PhongDaoTao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}