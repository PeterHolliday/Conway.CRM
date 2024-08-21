namespace Conway.CRM.Domain.Entities
{
    public class Opportunity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal EstimatedValue { get; set; }
        public DateTime ExpectedCloseDate { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid StageId { get; set; }
        public Stage Stage { get; set; }
    }
}
