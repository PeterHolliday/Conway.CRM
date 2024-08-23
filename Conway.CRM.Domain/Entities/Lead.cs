using System.ComponentModel.DataAnnotations;

namespace Conway.CRM.Domain.Entities
{
    public class Lead
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        public string? Email { get; set; }
        
        public string? PhoneNumber { get; set; }

        public string? MobileNumber { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsQualified { get; set; } = false;
    }
}
