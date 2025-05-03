using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
    {
        ProjectEntity MapModelToEntity(Project model);
    }

    public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
    {
        // Override för att använda din specifika mappning
        public override async Task<RepositoryResult<Project>> GetAsync(
            Expression<Func<ProjectEntity, bool>>? where = null,
            params Expression<Func<ProjectEntity, object>>[] includes)
        {
            IQueryable<ProjectEntity> query = _table;

            // Inkludera eventuella includes för relationer
            if (includes != null && includes.Length != 0)
                foreach (var include in includes)
                    query = query.Include(include);

            // Hämta den första matchande entiteten
            var entity = await query.FirstOrDefaultAsync(where!);
            if (entity == null)
                return new RepositoryResult<Project>
                { Succeeded = false, StatusCode = 404, Error = "Project not found." };

            // Använd din specifika mappning
            var result = MapEntityToModel(entity);

            return new RepositoryResult<Project>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = result
            };
        }

        // Genererat för att mappa om Status korrekt
        protected override Project MapEntityToModel(ProjectEntity entity)
        {
            return new Project
            {
                Id = entity.Id,
                ProjectName = entity.ProjectName,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Budget = entity.Budget,

                Client = entity.Client != null
                    ? new Client
                    {
                        Id = entity.Client.Id,
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

                //User = entity.User != null
                //    ? new User
                //    {
                //        Id = entity.User.Id,
                //        FirstName = entity.User.FirstName,
                //        LastName = entity.User.LastName,
                //        Email = entity.User.Email!
                //    }
                //    : null
            };
        }
        public ProjectEntity MapModelToEntity(Project model)
        {
            return new ProjectEntity
            {
                Id = model.Id,
                ProjectName = model.ProjectName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Budget = model.Budget,
                ClientId = model.Client?.Id!,
                StatusId = model.Status?.Id ?? 1
               
            };
        }
    }
}

    //public class ProjectRepository : BaseRepository<ProjectEntity, Project>, IProjectRepository
    //{
    //    public ProjectRepository(DataContext context) : base(context)
    //    {
    //    }


//    // Genererat av Chat GTP 4o efter problem med mappning av status. 
//    protected override Project MapEntityToModel(ProjectEntity entity)
//    {
//        return new Project
//        {
//            Id = entity.Id.ToString(),
//            ProjectName = entity.ProjectName,
//            Description = entity.Description,
//            StartDate = entity.StartDate,
//            EndDate = entity.EndDate,
//            Budget = entity.Budget,

//            Client = entity.Client != null
//                ? new Client
//                {
//                    Id = entity.Client.Id.ToString(),
//                    ClientName = entity.Client.ClientName,
//                }
//                : null,

//            Status = entity.Status != null
//                ? new Status
//                {
//                    Id = entity.Status.Id,
//                    StatusName = entity.Status.StatusName
//                }
//                : null,

//            User = entity.User != null
//                ? new User
//                {
//                    Id = entity.User.Id.ToString(),
//                    FirstName = entity.User.FirstName,
//                    LastName = entity.User.LastName,
//                    Email = entity.User.Email!
//                }
//                : null
//        };
//    }
//}
