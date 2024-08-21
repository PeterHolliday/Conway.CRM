namespace Conway.CRM.Domain.Entities
{
    public class Lead
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsQualified { get; set; } = false;
    }
}
