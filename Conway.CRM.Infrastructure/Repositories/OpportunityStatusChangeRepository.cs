using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class OpportunityStatusChangeRepository : IOpportunityStatusChangeRepository
    {
        private readonly CRMDbContext _context;

        public OpportunityStatusChangeRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OpportunityStatusChange>> GetAllOpportunityStatusChangesAsync()
        {
            return await _context.OpportunityStatusChanges.ToListAsync();
        }
    }
}
