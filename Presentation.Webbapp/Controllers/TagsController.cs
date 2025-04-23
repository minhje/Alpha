using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.WebApp.Controllers;

public class TagsController(DataContext context) : Controller
{
    private readonly DataContext _context = context;


    [HttpGet]
    public async Task<IActionResult> SearchTags(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var tags = await _context.Tags
         .Where(x => x.TagName.Contains(term))
         .Select(x => new { id = x.Id, tagName = x.TagName })
         .ToListAsync();

        return Json(tags);
    }

    [HttpGet]
    public async Task<IActionResult> SearchClients(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var clients = await _context.Clients
            .Where(c => c.ClientName.Contains(term))
            .Select(c => new { id = c.Id, clientName = c.ClientName })
            .ToListAsync();

        return Json(clients);
    }
}
