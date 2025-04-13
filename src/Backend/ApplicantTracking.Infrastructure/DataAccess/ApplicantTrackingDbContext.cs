using ApplicantTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicantTracking.Infrastructure.DataAccess;

public class ApplicantTrackingDbContext : DbContext
{
    public ApplicantTrackingDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Timeline> Timelines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicantTrackingDbContext).Assembly);
    }
}
