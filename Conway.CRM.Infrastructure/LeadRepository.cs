using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure
{
    public class LeadRepository : ILeadRepository
    {
        private readonly CRMDbContext _context;

        public LeadRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task AddLeadAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead != null)
            {
                _context.Leads.Remove(lead);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync()
        {
            return await _context.Leads.ToListAsync();
        }

        public async Task<Lead> GetLeadByIdAsync(Guid id)
        {
            return await _context.Leads.FindAsync(id);
        }

        public async Task UpdateLeadAsync(Lead lead)
        {
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
        }
    }
}
