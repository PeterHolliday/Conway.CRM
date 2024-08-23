namespace Conway.CRM.Domain.Entities
{
    public class Stage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public int Order { get; set; } 

        public List<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
}
