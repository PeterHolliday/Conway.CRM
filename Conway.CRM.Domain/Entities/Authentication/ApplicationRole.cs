namespace Conway.CRM.Domain.Entities.Authentication
{
    public class ApplicationRole
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
