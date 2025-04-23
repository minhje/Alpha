using Business.Services;
using Data.Contexts;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;
using Presentation.WebApp.ViewModels.Add;
using Presentation.WebApp.ViewModels.Edit;
using System;

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



    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        var addProjectFormData = model.MapTo<AddProjectFormData>();
        var result = await _projectService.CreateProjectAsync(addProjectFormData);

        return RedirectToAction("Index");

    }

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