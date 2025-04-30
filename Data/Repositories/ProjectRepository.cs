using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
  
  
}
public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
   

    
}
