namespace Conway.CRM.Domain.Entities
{
    public class OpportunityStatusChange
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OpportunityId { get; set; }
        public Opportunity Opportunity { get; set; }
        public Guid StageId { get; set; }
        public Stage Stage { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
