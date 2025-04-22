using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
