using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
