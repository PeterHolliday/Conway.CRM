using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly CRMDbContext _context;

        public PersonRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task AddPersonAsync(Person person)
        {
            await _context.Set<Person>().AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(Guid id)
        {
            var person = await _context.Set<Person>().FindAsync(id);
            if (person != null)
            {
                _context.Set<Person>().Remove(person);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _context.Set<Person>().Include(p => p.Status).ToListAsync();
        }

        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            return await _context.Set<Person>().FindAsync(id);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            _context.Set<Person>().Update(person);
            await _context.SaveChangesAsync();
        }
    }
}
