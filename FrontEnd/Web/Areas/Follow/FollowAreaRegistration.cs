using System.Web.Mvc;

namespace Web.Areas.Follow
{
    public class FollowAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Follow";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Follow_default",
                "Follow/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}