namespace Conway.CRM.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CompanyName { get; set; }
        
        public string? Email { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public string Address1 { get; set; }
        
        public string? Address2 { get; set; }
        
        public string? Address3 { get; set; }
        
        public string? Town { get; set; }
        
        public string? County { get; set; }
        
        public string Postcode { get; set; }
        
        public int InvoiceAccountNo { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public List<Opportunity> Opportunities { get; set; } = new List<Opportunity>();

        public int? ExternalRef {  get; set; }
    }
}
