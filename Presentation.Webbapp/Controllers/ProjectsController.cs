using Business.Models;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
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
        var projectViewModels = projectServiceResult.Result!
            .ToList();

        if (statusId.HasValue)
        {
            projectViewModels = projectViewModels
            .Where(p => p.Status != null && (int)p.Status.Id == statusId.Value)
            .ToList();

        }

        var viewModel = new ProjectViewModel(_clientService)
        {
            AddProjectFormData = new AddProjectViewModel(),
            EditProjectFormData = new EditProjectViewModel(),
            Projects = projectViewModels,

            AllCount = projectServiceResult.Result.Count(),
            StartedCount = projectServiceResult.Result.Count(p => p.Status != null && (int)p.Status.Id == 1),
            CompletedCount = projectServiceResult.Result.Count(p => p.Status != null && (int)p.Status.Id == 2)
        };

        return View(viewModel);
    }
    //[Route("projects")]
    //public async Task<IActionResult> Index()
    //{
    //    var projectServiceResult = await _projectService.GetProjectsAsync();
    //    var projectViewModels = projectServiceResult.Result!
    //        .ToList();

    //    var viewModel = new ProjectViewModel(_clientService)
    //    {
    //        AddProjectFormData = new AddProjectViewModel(),
    //        EditProjectFormData = new EditProjectViewModel(),
    //        Projects = projectViewModels
    //    };

    //    return View(viewModel);
    //}

    // genererat av chat GPT 4o för att försöka få mappningen av "selectedClientsIds" till "Clients" för att kunna skapa ett nytt projekt.
    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        Console.WriteLine($"Add method called at: {DateTime.UtcNow}");
        Console.WriteLine($"Project Name: {model.ProjectName}");
        Console.WriteLine($"Selected Client IDs: {string.Join(", ", model.SelectedClientIds)}");

        if (ModelState.IsValid)
        {
            Console.WriteLine("ModelState is valid. Proceeding with project creation.");
            var addProjectFormData = new AddProjectFormData
            {
                ProjectName = model.ProjectName,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
                Budget = model.Budget,
                SelectedClientIds = model.SelectedClientIds
            };

            var result = await _projectService.CreateProjectAsync(addProjectFormData);
            if (result.Succeeded)
            {
                Console.WriteLine("Project created successfully.");
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine($"Project creation failed: {result.Error}");
            }
        }
        else
        {
            Console.WriteLine("ModelState is invalid.");
        }

        return View("Index", new ProjectViewModel(_clientService)
        {
            AddProjectFormData = model,
            Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
        });
    }




    //[HttpPost]
    //public async Task<IActionResult> Add(AddProjectViewModel model)
    //{
    //    //ViewBag.Description = model.Description;
    //    Console.WriteLine($"Request received at: {DateTime.UtcNow}");

    //    if (ModelState.IsValid)
    //    {
    //        var addProjectFormData = new AddProjectFormData
    //        {
    //            ProjectName = model.ProjectName,
    //            StartDate = model.StartDate,
    //            EndDate = model.EndDate,
    //            Description = model.Description,
    //            Budget = model.Budget,
    //            SelectedClientIds = model.SelectedClientIds // Map selected client IDs
    //        };

    //        var result = await _projectService.CreateProjectAsync(addProjectFormData);
    //        if (result.Succeeded)
    //        {
    //            return RedirectToAction("Index");
    //        }
    //    }

    //    return View("Index", new ProjectViewModel(_clientService)
    //    {
    //        AddProjectFormData = model,
    //        Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
    //    });
    //}

    [HttpPost]
    public IActionResult Update(EditProjectViewModel model)
    {
        return RedirectToAction("Index");

    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        return RedirectToAction("Index");
    }
}