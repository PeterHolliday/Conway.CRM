namespace Conway.CRM.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public Guid CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; }
    }
}
