using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly CRMDbContext _context;

        public ContactRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task AddContactAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts.Include(c => c.Customer).ToListAsync();
        }

        public async Task DeleteContactAsync(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            return await _context.Contacts.Include(c => c.Customer)
                                          .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Contact>> GetContactsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Contacts
                                 .Where(c => c.CustomerId == customerId)
                                 .ToListAsync();
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }
    }
}
