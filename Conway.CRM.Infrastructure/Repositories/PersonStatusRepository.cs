using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class PersonStatusRepository : IPersonStatusRepository
    {
        private readonly CRMDbContext _context;

        public PersonStatusRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task AddPersonStatusAsync(PersonStatus personStatus)
        {
            await _context.Set<PersonStatus>().AddAsync(personStatus);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonStatusAsync(Guid id)
        {
            var personStatus = await _context.Set<PersonStatus>().FindAsync(id);
            if (personStatus != null)
            {
                _context.Set<PersonStatus>().Remove(personStatus);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PersonStatus>> GetAllPersonStatusesAsync()
        {
            return await _context.Set<PersonStatus>().ToListAsync();
        }

        public async Task<PersonStatus> GetPersonStatusByIdAsync(Guid id)
        {
            return await _context.Set<PersonStatus>().FindAsync(id);
        }

        public async Task UpdatePersonStatusAsync(PersonStatus personStatus)
        {
            _context.Set<PersonStatus>().Update(personStatus);
            await _context.SaveChangesAsync();
        }
    }
}
