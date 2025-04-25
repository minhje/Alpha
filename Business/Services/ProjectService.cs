using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ProjectResult<Project>> GetProjectAsync(string id);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, DataContext context) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;
    private readonly DataContext _context = context;

    // Genererat
    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null || string.IsNullOrEmpty(formData.ProjectName) || formData.SelectedClientIds == null || !formData.SelectedClientIds.Any())
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Required fields are missing."
            };
        }

        // Retrieve the ClientEntity from the database
        var clientId = formData.SelectedClientIds.FirstOrDefault();
        var clientEntity = await _projectRepository.GetClientByIdAsync(clientId!);
        if (clientEntity == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Client not found."
            };
        }

        // Create the ProjectEntity
        var projectEntity = new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Client = clientEntity, // Assign the full ClientEntity
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Description = formData.Description,
            Budget = formData.Budget
        };

        // Save the project to the database
        var result = await _projectRepository.AddAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }


    //public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    //{
    //    if (formData == null)
    //        return new ProjectResult
    //        { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

    //    var projectEntity = formData.MapTo<ProjectEntity>();
    //    var statusResult = await _statusService.GetStatusByIdAsync(1);
    //    var status = statusResult.Result;

    //    projectEntity.StatusId = status!.Id;

    //    var result = await _projectRepository.AddAsync(projectEntity);
    //    return result.Succeeded
    //        ? new ProjectResult { Succeeded = true, StatusCode = 201 }
    //        : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    //}

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
        (
            orderByDecending: true,
            sortBy: s => s.Created,
            where: null,
            include => include.User,
            include => include.Status,
            include => include.Client
        );

        // return response.MapTo<ProjectResult<IEnumerable<Project>>>();
        // or
        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync
        (
            where: x => x.Id == id,
            include => include.User,
            include => include.Status,
            include => include.Client
        );

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = true, StatusCode = 404, Error = $"Project '{id}' not found." };
    }
}