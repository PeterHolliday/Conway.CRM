using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain;
using Conway.CRM.Domain.Abstractions;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly CRMDbContext _context;

        [Inject] protected IStageRepository StageRepository { get; set; }

        public OpportunityRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOpportunityAsync(Opportunity opportunity)
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var stage = await _context.Stages.OrderBy(s => s.Order).FirstOrDefaultAsync();

                    await _context.Opportunities.AddAsync(opportunity);

                    var statusChange = new OpportunityStatusChange
                    {
                        OpportunityId = opportunity.Id,
                        StageId = stage.Id
                    };
                    await _context.OpportunityStatusChanges.AddAsync(statusChange);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;

                }
                catch (Exception ex) 
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOpportunityAsync(Guid id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity != null)
            {
                _context.Opportunities.Remove(opportunity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Opportunity>> GetOpportunitiesByCustomerIdAsync(Guid customerId)
        {
            return await _context.Opportunities
                                 .Include(o => o.Stage)
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }

        public async Task<Opportunity> GetOpportunityByIdAsync(Guid id)
        {
            return await _context.Opportunities.Include(o => o.Stage)
                                               .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> UpdateOpportunityAsync(Opportunity opportunity)
        {
            try
            {
                _context.Opportunities.Update(opportunity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        public async Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync()
        {
            return await _context.Opportunities.ToListAsync();
        }

        public async Task<IEnumerable<Opportunity>> GetAllOpportunitiesWithCustomersAsync()
        {
            return await _context.Opportunities
                                 .Include(c => c.Customer)
                                 .Include(s => s.Stage)
                                 .Include(p => p.AccountManager)
                                 .ToListAsync();
        }

        public async Task UpdateOpportunityStatusAsync(Guid opportunityId, Guid newStageId)
        {
            var opportunity = await _context.Opportunities.Include(o => o.Stage).FirstOrDefaultAsync(o => o.Id == opportunityId);
            if (opportunity == null) throw new Exception("Opportunity not found");

            var newStage = await _context.Stages.FindAsync(newStageId);
            if (newStage == null) throw new Exception("Stage not found");

            if (newStage.Order < opportunity.Stage.Order)
            {
                throw new InvalidOperationException("Cannot move to a previous stage.");
            }

            var statusChange = new OpportunityStatusChange
            {
                OpportunityId = opportunityId,
                StageId = newStageId
            };

            opportunity.StageId = newStageId;
            _context.Opportunities.Update(opportunity);
            await _context.OpportunityStatusChanges.AddAsync(statusChange);
            await _context.SaveChangesAsync();
        }

        
    }
}
