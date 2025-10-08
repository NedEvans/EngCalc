using EngineeringCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringCalc.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Project> Projects { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Calculation> Calculations { get; set; }
    public DbSet<CalculationRevision> CalculationRevisions { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<CardInstance> CardInstances { get; set; }
    public DbSet<AppConstant> AppConstants { get; set; }
    public DbSet<GlobalConstant> GlobalConstants { get; set; }
    public DbSet<MaterialLibrary> MaterialLibrary { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Calculation>()
            .HasOne(c => c.CurrentRevision)
            .WithMany()
            .HasForeignKey(c => c.CurrentRevisionId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure indexes for common queries
        modelBuilder.Entity<Project>()
            .HasIndex(p => p.ProjectName);

        modelBuilder.Entity<Job>()
            .HasIndex(j => new { j.ProjectId, j.JobName });

        modelBuilder.Entity<Calculation>()
            .HasIndex(c => new { c.JobId, c.CalculationTitle });

        modelBuilder.Entity<Card>()
            .HasIndex(c => c.CardType);

        modelBuilder.Entity<AppConstant>()
            .HasIndex(a => a.ConstantName);

        modelBuilder.Entity<GlobalConstant>()
            .HasIndex(g => new { g.JobId, g.ConstantName });

        modelBuilder.Entity<MaterialLibrary>()
            .HasIndex(m => new { m.MaterialType, m.Grade });
    }
}
