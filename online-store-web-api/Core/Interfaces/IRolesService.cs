using Core.Entities;

namespace Core.Interfaces
{
    public interface IRolesService
    {
        Task Create(string roleName);
        Task Delete(string roleName);
        Task AddToRole(string userId, string roleName);
        Task RemoveFromRole(string userId, string roleName);
        Task<IEnumerable<Role>> GetAll();
        Task<IEnumerable<string>> GetUserRoles(string userId);
    }
}