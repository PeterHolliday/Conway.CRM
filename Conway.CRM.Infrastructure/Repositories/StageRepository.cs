using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Conway.CRM.Infrastructure.Repositories
{
    public class StageRepository : IStageRepository
    {
        private readonly CRMDbContext _context;

        public StageRepository(CRMDbContext context)
        {
            _context = context;
        }

        public async Task AddStageAsync(Stage stage)
        {
            await _context.Stages.AddAsync(stage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStageAsync(Guid id)
        {
            var stage = await _context.Stages.FindAsync(id);
            if (stage != null)
            {
                _context.Stages.Remove(stage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Stage>> GetAllStagesAsync()
        {
            return await _context.Stages.OrderBy(s => s.Order).ToListAsync();
        }

        public async Task<Stage> GetStageByIdAsync(Guid id)
        {
            return await _context.Stages.FindAsync(id);
        }

        public async Task UpdateStageAsync(Stage stage)
        {
            _context.Stages.Update(stage);
            await _context.SaveChangesAsync();
        }
    }
}
