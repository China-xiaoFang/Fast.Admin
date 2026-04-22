using Fast.Deploy.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Fast.Deploy.Server.Data;

public class DeployDbContext : DbContext
{
    public DeployDbContext(DbContextOptions<DeployDbContext> options) : base(options)
    {
    }

    public DbSet<AppModel> Apps { get; set; }
    public DbSet<VersionModel> Versions { get; set; }
    public DbSet<NodeModel> Nodes { get; set; }
    public DbSet<DeploymentModel> Deployments { get; set; }
    public DbSet<DeployLogModel> DeployLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppModel>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasMany(x => x.Versions).WithOne(v => v.App).HasForeignKey(v => v.AppId);
            e.HasMany(x => x.Deployments).WithOne(d => d.App).HasForeignKey(d => d.AppId);
        });

        modelBuilder.Entity<VersionModel>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Version).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<NodeModel>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Ip).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<DeploymentModel>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.AppVersion).WithMany().HasForeignKey(x => x.VersionId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Logs).WithOne(l => l.Deployment).HasForeignKey(l => l.DeploymentId);
        });

        modelBuilder.Entity<DeployLogModel>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Level).HasMaxLength(10);
        });

        base.OnModelCreating(modelBuilder);
    }
}
