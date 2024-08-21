using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> GetContactByIdAsync(Guid id);
        Task<IEnumerable<Contact>> GetContactsByCustomerIdAsync(Guid customerId);
        Task AddContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(Guid id);
        Task<IEnumerable<Contact>> GetAllContactsAsync();
    }
}
