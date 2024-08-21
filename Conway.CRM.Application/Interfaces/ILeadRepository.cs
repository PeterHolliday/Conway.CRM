using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface ILeadRepository
    {
        Task<Lead> GetLeadByIdAsync(Guid id);
        Task<IEnumerable<Lead>> GetAllLeadsAsync();
        Task AddLeadAsync(Lead lead);
        Task UpdateLeadAsync(Lead lead);
        Task DeleteLeadAsync(Guid id);
    }
}
