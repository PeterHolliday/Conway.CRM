using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonByIdAsync(Guid id);
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(Guid id);
    }
}
