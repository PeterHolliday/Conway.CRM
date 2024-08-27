namespace Conway.CRM.Domain.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        
        public string Surname { get; set; }
        
        public string? Location { get; set; }
        
        public int? IconId { get; set; }
        
        public string? ShortName { get; set; }

        public string Department { get; set; }

        public string? FirstName { get; set; }
        
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        
        public Guid? StatusId { get; set; }

        public PersonStatus? Status { get; set; }
        
        public string? TownWorkingAt { get; set; }
        
        public string FullName => $"{FirstName} {Surname}";

        public int ExternalRef { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set;} = DateTime.Now;

        public List<Opportunity>? Opportunities { get; set; }
    }
}
