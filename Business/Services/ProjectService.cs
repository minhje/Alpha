using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<IEnumerable<SelectListItem>> GetClientSelectListAsync();
    Task<ProjectResult<EditProjectFormData>> GetEditFormDataAsync(string id);
}
public class ProjectService(IProjectRepository projectRepository, IClientRepository clientRepository, IClientService clientService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IClientService _clientService = clientService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        var projectEntity = new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget,
            StatusId = 1,
            ClientId = formData.SelectedClientId ?? "",
            Created = DateTime.UtcNow
        };

        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
        (
            orderByDecending: true,
            sortBy: s => s.Created,
            where: null,
            include => include.User!,
            include => include.Status,
            include => include.Client
        );

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync(
            where: p => p.Id == id,
            include => include.User!,
            include => include.Status,
            include => include.Client
        );

        if (response.Succeeded)
        {
            var project = response.Result;

            var projectName = project!.ProjectName;
            var description = project.Description;
            var startDate = project.StartDate;
            var endDate = project.EndDate;
            var budget = project.Budget;
            var status = project.Status?.Id;
            var clientId = project.Client?.Id;

            return new ProjectResult<Project>
            { Succeeded = true, StatusCode = 200, Result = project
            };
        }
        else
        {
            return new ProjectResult<Project>
            { Succeeded = false, StatusCode = 404, Error = "Project not found."
            };
        }
    }

    public async Task<ProjectResult<EditProjectFormData>> GetEditFormDataAsync(string id)
    {
        var projectResult = await _projectRepository.GetAsync(
            where: p => p.Id == id,
            include => include.Client
        );

        if (!projectResult.Succeeded || projectResult.Result == null)
            return new ProjectResult<EditProjectFormData> { Succeeded = false, StatusCode = 404, Error = "Project not found." };

        var project = projectResult.Result;

        var clientOptions = (await GetClientSelectListAsync()).ToList();

        var formData = new EditProjectFormData
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            SelectedClientId = project.Client.Id,
            ClientOptions = clientOptions
        };

        return new ProjectResult<EditProjectFormData>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = formData
        };
    }

    public async Task<IEnumerable<SelectListItem>> GetClientSelectListAsync()
    {
        var result = await _clientService.GetClientsAsync();

        if (!result.Succeeded || result.Result == null)
            return Enumerable.Empty<SelectListItem>();

        return result.Result
            .Select(client => new SelectListItem
            {
                Value = client.Id,
                Text = client.ClientName
            });
    }

}