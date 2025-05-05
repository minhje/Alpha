using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<UserAdressEntity> UserAdresses { get; set; }
    public virtual DbSet<ProjectEntity> Projects { get; set; }
    public virtual DbSet<StatusEntity> Statuses { get; set; }
    //public virtual DbSet<TagEntity> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Status)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StatusEntity>().HasData(
            new StatusEntity { Id = 1, StatusName = "Started" },
            new StatusEntity { Id = 2, StatusName = "Completed" }
        );

        modelBuilder.Entity<ClientEntity>().HasData(
            new ClientEntity { Id = "1", ClientName = "GitLab Inc." },
            new ClientEntity { Id = "2", ClientName = "Bitbucket Inc." },
            new ClientEntity { Id = "3", ClientName = "Driveway Inc." },
            new ClientEntity { Id = "5", ClientName = "Slack Technologies Inc." }
        );
    }
}



