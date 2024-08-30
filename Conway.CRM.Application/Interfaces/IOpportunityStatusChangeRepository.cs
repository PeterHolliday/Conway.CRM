using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface IOpportunityStatusChangeRepository
    {
        Task<IEnumerable<OpportunityStatusChange>> GetAllOpportunityStatusChangesAsync();
    }
}
