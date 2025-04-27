using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
    new Task<RepositoryResult<bool>> AddAsync(ProjectEntity entity);
    Task<RepositoryResult<bool>> GetByIdAsync(string clientId);
}
public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
    public new async Task<RepositoryResult<bool>> AddAsync(ProjectEntity entity)
    {
        try
        {
            _context.Projects.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, Result = true };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<bool> { Succeeded = false, Error = ex.Message, Result = false };
        }
    }

    public async Task<RepositoryResult<bool>> GetByIdAsync(string clientId)
    { 
        try
        {
            await _context.Clients.FindAsync(clientId);
            return new RepositoryResult<bool> { Succeeded = true, Result = true };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<bool> { Succeeded = false, Error = ex.Message, Result = false };
        }
    }
}
