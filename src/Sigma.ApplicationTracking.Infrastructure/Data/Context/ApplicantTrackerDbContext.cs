using Microsoft.EntityFrameworkCore;
using Sigma.ApplicationTracking.Core.Entities;

namespace Sigma.ApplicationTracking.Infrastructure.Data.Context
{
    public class ApplicantTrackerDbContext : DbContext
    {
        public ApplicantTrackerDbContext(DbContextOptions<ApplicantTrackerDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Applicant> Applicants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.ToTable("Applicants", "tracker");
                entity.HasIndex(e => e.Email, "IDX_Applicants_Email")
                      .IsUnique();
            });
        }
    }

}
