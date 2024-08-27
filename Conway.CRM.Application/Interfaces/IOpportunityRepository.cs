using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface IOpportunityRepository
    {
        Task<Opportunity> GetOpportunityByIdAsync(Guid id);
        Task<IEnumerable<Opportunity>> GetOpportunitiesByCustomerIdAsync(Guid customerId);
        Task AddOpportunityAsync(Opportunity opportunity);
        Task UpdateOpportunityAsync(Opportunity opportunity);
        Task DeleteOpportunityAsync(Guid id);
        Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync();
        Task<IEnumerable<Opportunity>> GetAllOpportunitiesWithCustomersAsync();
        Task UpdateOpportunityStatusAsync(Guid opportunityId, Guid newStageId);
    }
}
