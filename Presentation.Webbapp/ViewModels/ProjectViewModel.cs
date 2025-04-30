using Business.Services;
using Domain.Dtos;
using Domain.Models;
using Presentation.WebApp.ViewModels.Add;
using Presentation.WebApp.ViewModels.Edit;

namespace Presentation.WebApp.ViewModels;

public class ProjectViewModel(IClientService clientService)
{
    private readonly IClientService _clientService = clientService;

    public IEnumerable<Project> Projects { get; set; } = [];
    public AddProjectFormData AddProjectFormData { get; set; } = new();
    public EditProjectViewModel EditProjectFormData { get; set; } = new();

    public int AllCount { get; set; } 
    public int CompletedCount { get; set; }
    public int StartedCount { get; set; }
}