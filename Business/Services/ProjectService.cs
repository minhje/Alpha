using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<IEnumerable<SelectListItem>> GetClientSelectListAsync();
}
public class ProjectService(IProjectRepository projectRepository, IClientService clientService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
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
        var response = await _projectRepository.GetAsync
        (
            where: p => p.Id == id,
            include => include.User!,
            include => include.Status,
            include => include.Client
        );



        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = true, StatusCode = 404, Error = $"Project not found." };
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