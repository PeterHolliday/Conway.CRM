namespace Conway.CRM.Domain.Entities.Authentication
{
    public class ApplicationUserRole
    {
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid RoleId { get; set; }

        public ApplicationRole Role { get; set; }
    }
}
