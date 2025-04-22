using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

public class HomeController : Controller
{
    [Route("admin/home")]
    public IActionResult Index()
    {
        return View();
    }
}