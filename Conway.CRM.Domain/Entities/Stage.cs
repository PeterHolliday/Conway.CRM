namespace Conway.CRM.Domain.Entities
{
    public class Stage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Order { get; set; } // Order in the pipeline, e.g., 1 for "Prospecting", 2 for "Qualification", etc.
    }
}
