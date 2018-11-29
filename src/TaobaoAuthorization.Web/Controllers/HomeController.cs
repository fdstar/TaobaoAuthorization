using Microsoft.AspNetCore.Mvc;

namespace TaobaoAuthorization.Web.Controllers
{
    public class HomeController : TaobaoAuthorizationControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}