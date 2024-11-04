using Microsoft.AspNetCore.Mvc;

namespace Future.Bangla.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult OrderIndex()
        {
            return View();
        }
    }
}
