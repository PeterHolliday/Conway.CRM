namespace Conway.CRM.Domain.Entities.Authentication
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }

        public string AzureAdUserId { get; set; }

        public string DisplayName { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string Email {  get; set; }
    }
}
