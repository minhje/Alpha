using Business.Models;
using Business.Services;
using Data.Contexts;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;
using Presentation.WebApp.ViewModels.Add;
using Presentation.WebApp.ViewModels.Edit;

namespace Presentation.WebApp.Controllers;

public class ProjectsController(IProjectService projectService, DataContext context, IClientService clientService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly DataContext _context = context;
    private readonly IClientService _clientService = clientService;

    [Route("projects")]
    public async Task<IActionResult> Index()
    {
        var projectServiceResult = await _projectService.GetProjectsAsync();
        var projectViewModels = projectServiceResult.Result!
            .ToList();

        var viewModel = new ProjectViewModel(_clientService)
        {
            AddProjectFormData = new AddProjectViewModel(),
            EditProjectFormData = new EditProjectViewModel(),
            Projects = projectViewModels
        };

        return View(viewModel);
    }

    // genererat av chat GPT 4o för att försöka få mappningen av "selectedClientsIds" till "Clients" för att kunna skapa ett nytt projekt. Inte fått det att fungera än. 
    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        Console.WriteLine($"SelectedClientIds: {string.Join(",", model.SelectedClientIds)}");

        if (ModelState.IsValid)
        {
            if (model.SelectedClientIds != null)
            {
                var clientId = model.SelectedClientIds.First();
                var clientResult = await _clientService.GetClientByIdAsync(clientId);

                if (!clientResult.Succeeded || clientResult.Result == null)
                {
                    ModelState.AddModelError("Client", clientResult.Error ?? "The selected client does not exist.");
                    return View("Index", new ProjectViewModel(_clientService)
                    {
                        AddProjectFormData = model,
                        Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
                    });
                }

                var client = clientResult.Result.FirstOrDefault();
                if (client == null)
                {
                    ModelState.AddModelError("Client", "The selected client does not exist.");
                    return View("Index", new ProjectViewModel(_clientService)
                    {
                        AddProjectFormData = model,
                        Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
                    });
                }

                model.Client = client;
            }
            else
            {
                ModelState.AddModelError("Client", "A client must be selected.");
                return View("Index", new ProjectViewModel(_clientService)
                {
                    AddProjectFormData = model,
                    Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
                });
            }

            var addProjectFormData = new AddProjectFormData
            {
                ProjectName = model.ProjectName,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
                Budget = model.Budget,
                Client = model.Client
            };

            var result = await _projectService.CreateProjectAsync(addProjectFormData);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, result.Error ?? "An unknown error occurred.");
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
    //    Console.WriteLine($"SelectedClientIds: {string.Join(",", model.SelectedClientIds)}");

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

    //        ModelState.AddModelError(string.Empty, result.Error);
    //    }

    //    return View("Index", new ProjectViewModel(_clientService)
    //    {
    //        AddProjectFormData = model,
    //        Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
    //    });
    //}

    //[HttpPost]
    //public async Task<IActionResult> Add(AddProjectFormData model)
    //{
    //    if (!ModelState.IsValid)
    //    {

    //        return View("Index", new ProjectViewModel(_clientService)
    //        {
    //            AddProjectFormData = model,
    //            Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
    //        });
    //    }

    //    var addProjectFormData = model.MapTo<AddProjectFormData>();
    //    var result = await _projectService.CreateProjectAsync(addProjectFormData);

    //    if (!result.Succeeded)
    //    {
    //        // Handle failure (e.g., show error message)
    //        ModelState.AddModelError("", "Failed to add project.");
    //        return View("Index", new ProjectViewModel(_clientService)
    //        {
    //            AddProjectFormData = model,
    //            Projects = (await _projectService.GetProjectsAsync()).Result!.ToList()
    //        });
    //    }

    //    return RedirectToAction("Index");
    //}

    //var addProjectFormData = model.MapTo<AddProjectFormData>();
    //var result = await _projectService.CreateProjectAsync(addProjectFormData);

    //return RedirectToAction("Index");

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