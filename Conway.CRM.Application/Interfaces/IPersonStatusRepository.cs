using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface IPersonStatusRepository
    {
        Task<PersonStatus> GetPersonStatusByIdAsync(Guid id);
        Task<IEnumerable<PersonStatus>> GetAllPersonStatusesAsync();
        Task AddPersonStatusAsync(PersonStatus personStatus);
        Task UpdatePersonStatusAsync(PersonStatus personStatus);
        Task DeletePersonStatusAsync(Guid id);
    }
}
