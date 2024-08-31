using Conway.CRM.Domain.Entities;
using Conway.CRM.Domain.Entities.Authentication;
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

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Status)
                .WithMany(p => p.People)
                .HasForeignKey(p => p.StatusId);

            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.AccountManager)
                .WithMany(p => p.Opportunities)
                .HasForeignKey(p => p.AccountManagerId);

            modelBuilder.Entity<OpportunityStatusChange>()
               .HasOne(osc => osc.Opportunity)
               .WithMany(o => o.OpportunityStatusChanges)
               .HasForeignKey(osc => osc.OpportunityId)
               .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            modelBuilder.Entity<OpportunityStatusChange>()
                .HasOne(osc => osc.Stage)
                .WithMany()
                .HasForeignKey(osc => osc.StageId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            modelBuilder.Entity<ApplicationUserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<Opportunity> Opportunities { get; set; }

        public DbSet<Stage> Stages { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<PersonStatus> PersonStatuses { get; set; }

        public DbSet<OpportunityStatusChange> OpportunityStatusChanges { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<ApplicationUserRole> UserRoles { get; set; }
    }
}
