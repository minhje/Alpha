using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels.Add;
using Presentation.WebApp.ViewModels.Edit;

namespace Presentation.WebApp.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    public async Task<IActionResult> Index()
    {
        return View();
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