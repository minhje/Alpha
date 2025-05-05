using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.WebApp.ViewModels;
using Presentation.WebApp.ViewModels.Add;

namespace Presentation.WebApp.Controllers;

public class ProjectsController(IProjectService projectService, IStatusService statusService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IStatusService _statusService = statusService;


    // Hjälp tagen av ChatGTP-4o för att filtrera efter status. 
    [Route("projects")]
    public async Task<IActionResult> Index(int? statusId, AddProjectViewModel model, EditProjectViewModel editModel)
    {
        var projectServiceResult = await _projectService.GetProjectsAsync();
        var allProjects = projectServiceResult.Result!.ToList();

        ViewBag.Description = model.Description;

        var filteredProjects = statusId.HasValue
            ? allProjects.Where(p => p.Status?.Id == statusId.Value).ToList()
            : allProjects;

        var viewModel = new ProjectViewModel
        {
            AddProjectFormData = new AddProjectFormData
            {
                ClientOptions = await _projectService.GetClientSelectListAsync()
            },

            EditProjectFormData = new EditProjectFormData
            { 
                ClientOptions = await _projectService.GetClientSelectListAsync(),
                StatusOptions = await _statusService.GetStatusSelectListAsync()
            },
            
            Projects = filteredProjects,
            AllCount = allProjects.Count,
            StartedCount = allProjects.Count(p => p.Status?.Id == 1),
            CompletedCount = allProjects.Count(p => p.Status?.Id == 2)
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProjectFormData formData)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Where(x => x.Value?.Errors.Count > 0).ToDictionary(x => x.Key, x => x.Value?.Errors.Select(x => x.ErrorMessage).ToArray());
            return BadRequest(new { success = false, errors });
        }

        var result = await _projectService.CreateProjectAsync(formData);

        if (result.Succeeded)
        {
            return Ok();
        }

        return Conflict();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var result = await _projectService.GetEditFormDataAsync(id);

        if (!result.Succeeded)
        {
            return NotFound(new { success = false, message = result.Error });
        }

        return Json(new { success = true, data = result.Result });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditProjectFormData formData)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(x => x.Key, x => x.Value?.Errors.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new { success = false, errors });
        }

        var result = await _projectService.UpdateProjectAsync(formData);
        var statusResult = await _statusService.GetStatusesAsync();

        if (statusResult.Succeeded && statusResult.Result != null)
        {
            formData.StatusOptions = statusResult.Result.Select(status => new SelectListItem
            {
                Value = status.Id.ToString(),
                Text = status.StatusName
            });
        }

        if (result.Succeeded)
        {
            return Ok();
        }

        return Conflict();
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        return RedirectToAction("Index");
    }
}