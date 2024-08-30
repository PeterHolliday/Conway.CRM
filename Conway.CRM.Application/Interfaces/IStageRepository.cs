using Conway.CRM.Domain.Entities;

namespace Conway.CRM.Application.Interfaces
{
    public interface IStageRepository
    {
        Task<Stage> GetStageByIdAsync(Guid id);
        Task<IEnumerable<Stage>> GetAllStagesAsync();
        Task AddStageAsync(Stage stage);
        Task UpdateStageAsync(Stage stage);
        Task DeleteStageAsync(Guid id);
        Task<bool> IsStageOrderUniqueAsync(int order);
        Task<Stage> GetDefaultStageAsync();
    }
}
