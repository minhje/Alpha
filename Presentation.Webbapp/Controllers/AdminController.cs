using Business.Services;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class AdminController(IUserService userService, IProjectService projectService, IClientService clientService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Projects()
    {
        var projectResult = await _projectService.GetProjectsAsync();
        var projects = projectResult.Result;

        var viewModel = new ProjectViewModel()
        {
            Projects = projects ?? [],
            AddProjectFormData = new AddProjectFormData(),
            EditProjectFormData = new EditProjectFormData()
        };

        return View(viewModel);

    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Members()
    {
        var memberResult = await _userService.GetUsersAsync();
        var members = memberResult.Result;
        return View(members);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Clients()
    {
        var clientResult = await _clientService.GetClientsAsync();
        var clients = clientResult.Result;
        return View(clients);
    }
}