using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllCustomersWithContactsAsync();
    }
}
