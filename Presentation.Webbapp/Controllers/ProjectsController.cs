using Business.Models;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.ViewModels;
using Presentation.WebApp.ViewModels.Add;
using Presentation.WebApp.ViewModels.Edit;
using System.Text.Json;

namespace Presentation.WebApp.Controllers;

public class ProjectsController(IProjectService projectService, DataContext context, IClientService clientService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly DataContext _context = context;
    private readonly IClientService _clientService = clientService;


    [Route("projects")]
    public async Task<IActionResult> Index(int? statusId)
    {
        var projectServiceResult = await _projectService.GetProjectsAsync();
        var allProjects = projectServiceResult.Result!.ToList();

        // Filtrera projekten baserat på statusId
        var filteredProjects = statusId.HasValue
            ? allProjects.Where(p => p.Status?.Id == statusId.Value).ToList()
            : allProjects;

        foreach (var project in allProjects)
        {
            Console.WriteLine($"Project: {project.ProjectName}, Status: {project.Status?.Id}");
        }

        // Skapa ViewModel med dropdown-data
        var viewModel = new ProjectViewModel(_clientService)
        {
            AddProjectFormData = new AddProjectViewModel
            {
                ClientOptions = _context.Clients
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id,
                        Text = c.ClientName
                    }).ToList()
            },
            EditProjectFormData = new EditProjectViewModel(),
            Projects = filteredProjects,

            AllCount = allProjects.Count,
            StartedCount = allProjects.Count(p => p.Status?.Id == 1),
            CompletedCount = allProjects.Count(p => p.Status?.Id == 2)
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Add(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ClientOptions = _context.Clients
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ClientName
                }).ToList();

            return View(model);
        }

        var addProjectFormData = new AddProjectFormData
        {
            ProjectName = model.ProjectName,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Description = model.Description,
            Budget = model.Budget,
            SelectedClientId = model.SelectedClientId 
        };

        var result = _projectService.CreateProjectAsync(addProjectFormData).Result;

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", result.Error);
        model.ClientOptions = _context.Clients
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.ClientName
            }).ToList();

        return View(model);
    }



    [HttpGet]
    public IActionResult Add()
    {
        var model = new AddProjectViewModel
        {
            ClientOptions = _context.Clients
                .Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.ClientName
                }).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var result = await _projectService.GetProjectAsync(id);

        if (!result.Succeeded || result.Result == null)
            return NotFound();

        var project = result.Result;

        var allClients = await _context.Clients.ToListAsync(); // felsökning

        if (!allClients.Any())
        {
            Console.WriteLine("❌ Inga klienter hittades i databasen.");
        }
        else
        {
            Console.WriteLine($"✅ {allClients.Count} klienter hittades.");
        }


        // Fyll modellen du använder i formuläret
        var model = new EditProjectViewModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            SelectedClientId = project.Client?.Id,
            Clients = await _context.Clients.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.ClientName,
            }).ToListAsync()
        };

        return PartialView("_EditProjectPartial", model);
    }



    [HttpPost]
    public async Task<IActionResult> Edit(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.Id);
        if (project == null)
        {
            return NotFound();
        }

        // Uppdatera projektet med data från formuläret
        project.ProjectName = model.ProjectName;
        project.Description = model.Description;
        project.StartDate = model.StartDate;
        project.EndDate = model.EndDate;
        project.Budget = model.Budget;
        project.ClientId = model.SelectedClientId; // Se till att klientens ID sparas korrekt

        // Om en bild är vald, uppdatera den (om det behövs)
        if (model.ProjectImage != null)
        {
            // Här kan du lägga till logik för att spara bilden och uppdatera path
            // project.ImagePath = "new_image_path";
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");  // Eller den sida du vill navigera till efter uppdatering
    }



    [HttpPost]
    public IActionResult Delete(string id)
    {
        return RedirectToAction("Index");
    }
}