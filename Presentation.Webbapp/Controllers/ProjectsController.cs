using Business.Services;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;
using Presentation.WebApp.ViewModels.Add;

namespace Presentation.WebApp.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [Route("projects")]
    public async Task<IActionResult> Index(int? statusId, AddProjectViewModel model, EditProjectViewModel editModel)
    {
        var projectServiceResult = await _projectService.GetProjectsAsync();
        var allProjects = projectServiceResult.Result!.ToList();

        ViewBag.Description = model.Description;


        // Filtrera projekten baserat på statusId
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
                ClientOptions = await _projectService.GetClientSelectListAsync() 
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
        var projectServiceResult = await _projectService.GetProjectAsync(id);
        var project = projectServiceResult.Result;
        if (project == null)
        {
            return NotFound();
        }
        var editModel = new EditProjectViewModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            ClientOptions = await _projectService.GetClientSelectListAsync(),
            SelectedClientId = project.Client?.Id
        };
        return View(editModel);
    }

    //[HttpPost]
    //public async Task<IActionResult> Edit(EditProjectFormData formData)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        var errors = ModelState.Where(x => x.Value?.Errors.Count > 0).ToDictionary(x => x.Key, x => x.Value?.Errors.Select(x => x.ErrorMessage).ToArray());
    //        return BadRequest(new { success = false, errors });
    //    }
    //    //var result = await _projectService.EditProjectAsync(formData);
    //    if (result.Succeeded)
    //    {
    //        return Ok();
    //    }
    //    return Conflict();

    //}

    [HttpPost]
    public IActionResult Delete(string id)
    {
        return RedirectToAction("Index");
    }
}