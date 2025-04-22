using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.WebApp.Controllers
{
    public class ClientsController(DataContext context) : Controller
    {
        private readonly DataContext _context = context;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> SearchClients(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var clients = await _context.Clients
              .Where(x => x.ClientName.Contains(term))
              .Select(x => new { x.Id, x.ClientName })
              .ToListAsync();

            return Json(clients);
            
        }
    }
}
