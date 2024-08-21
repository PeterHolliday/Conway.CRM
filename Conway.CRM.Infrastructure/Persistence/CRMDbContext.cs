using Conway.CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Persistence
{
    public class CRMDbContext : DbContext
    {
        public CRMDbContext(DbContextOptions<CRMDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Contact> Contacts { get; set; }
    }
}
