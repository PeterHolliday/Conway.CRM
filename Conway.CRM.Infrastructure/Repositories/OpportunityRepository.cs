﻿using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly CRMDbContext _context;

        public OpportunityRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task AddOpportunityAsync(Opportunity opportunity)
        {
            await _context.Opportunities.AddAsync(opportunity);
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

        public async Task UpdateOpportunityAsync(Opportunity opportunity)
        {
            _context.Opportunities.Update(opportunity);
            await _context.SaveChangesAsync();
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
                                 .ToListAsync();
        }
    }
}
