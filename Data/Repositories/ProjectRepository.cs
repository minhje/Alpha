using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories
{
    public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
    {
    }

    public class ProjectRepository : BaseRepository<ProjectEntity, Project>, IProjectRepository
    {
        public ProjectRepository(DataContext context) : base(context)
        {
        }


        // Genererat av Chat GTP 4o efter problem med mappning av status. 
        protected override Project MapEntityToModel(ProjectEntity entity)
        {
            return new Project
            {
                Id = entity.Id.ToString(),
                ProjectName = entity.ProjectName,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Budget = entity.Budget,

                Client = entity.Client != null
                    ? new Client
                    {
                        Id = entity.Client.Id.ToString(),
                        ClientName = entity.Client.ClientName,
                    }
                    : null,

                Status = entity.Status != null
                    ? new Status
                    {
                        Id = entity.Status.Id,
                        StatusName = entity.Status.StatusName
                    }
                    : null,

                User = entity.User != null
                    ? new User
                    {
                        Id = entity.User.Id.ToString(),
                        FirstName = entity.User.FirstName,
                        LastName = entity.User.LastName,
                        Email = entity.User.Email!
                    }
                    : null
            };
        }
    }
}