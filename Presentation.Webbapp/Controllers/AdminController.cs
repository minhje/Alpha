using Business.Services;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;
using Presentation.WebApp.ViewModels.Add;
using Presentation.WebApp.ViewModels.Edit;

namespace Presentation.WebApp.Controllers;

//[Authorize]
//[Route("admin")]
public class AdminController(IUserService userService, IProjectService projectService, IClientService clientService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;

    //[Route("dashboard")]
    public IActionResult Index()
    {
        return View();
    }

    //[Route("projects/index")]
    public async Task<IActionResult> Projects()
    {
        var projectResult = await _projectService.GetProjectsAsync();
        var projects = projectResult.Result;

        var viewModel = new ProjectViewModel(_clientService)
        {
            Projects = projects ?? [],
            AddProjectFormData = new AddProjectFormData(),
            EditProjectFormData = new EditProjectViewModel()
        };

        return View(viewModel);
    }

    //[Authorize(Roles = "admin")]
    //[Route("members")]
    public async Task<IActionResult> Members()
    {
        var memberResult = await _userService.GetUsersAsync();
        var members = memberResult.Result;
        return View(members);
    }

    //[Authorize(Roles = "admin")]
    //[Route("clients")]
    public async Task<IActionResult> Clients()
    {
        var clientResult = await _clientService.GetClientsAsync();
        var clients = clientResult.Result;
        return View(clients);
    }
}