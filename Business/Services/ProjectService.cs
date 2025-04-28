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
    Task<ProjectResult<Project>> GetProjectAsync();
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, DataContext context) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;
    private readonly DataContext _context = context;


    // Genererat av Chat GPT 4o för att kunna skapa ett projekt, haft problem med att kunna koppla SelectedClientIds till Clients i databasen. 
    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null || string.IsNullOrEmpty(formData.ProjectName) || formData.SelectedClientIds == null || !formData.SelectedClientIds.Any())
        {
            return new ProjectResult
            { Succeeded = false, StatusCode = 400, Error = "Required fields are missing." };
        }

        // Retrieve all clients based on SelectedClientIds
        var clientEntities = await _context.Clients
            .Where(c => formData.SelectedClientIds.Contains(c.Id))
            .ToListAsync();

        if (!clientEntities.Any())
        {
            return new ProjectResult
            { Succeeded = false, StatusCode = 404, Error = "No valid clients found for the provided IDs." };
        }

        // Retrieve the default status
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        if (status == null)
        {
            return new ProjectResult
            { Succeeded = false, StatusCode = 500, Error = "Default status could not be retrieved." };
        }

        // Create the ProjectEntity
        var projectEntity = new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget,
            StatusId = status.Id,
            ClientId = clientEntities.First().Id, // Assuming one client per project
            Created = DateTime.UtcNow
        };

        // Save the project to the database
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


    public async Task<ProjectResult<Project>> GetProjectAsync()
    {
        var response = await _projectRepository.GetAsync
        (
            where: null,
            include => include.User!,
            include => include.Status,
            include => include.Client
        );

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = true, StatusCode = 404, Error = $"Project not found." };
    }

}