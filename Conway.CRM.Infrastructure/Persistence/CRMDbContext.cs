using Conway.CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Persistence
{
    public class CRMDbContext : DbContext
    {
        public CRMDbContext(DbContextOptions<CRMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Stage>()
                .HasIndex(c => c.Order)
                .IsUnique();

            modelBuilder.Entity<Contact>()
                    .HasOne(c => c.Customer)
                    .WithMany(c => c.Contacts)
                    .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.Customer)
                .WithMany(o => o.Opportunities)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.Stage)
                .WithMany(o => o.Opportunities)
                .HasForeignKey(o => o.StageId);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<Opportunity> Opportunities { get; set; }

        public DbSet<Stage> Stages { get; set; }
    }
}
