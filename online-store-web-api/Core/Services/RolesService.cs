using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Core.Services
{
    public class RolesService(RoleManager<Role> roleManager, 
                              UserManager<User> userManager) : IRolesService
    {
        public async Task Create(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                throw new HttpException(ErrorMessages.RoleAlreadyExists, HttpStatusCode.BadRequest);

            var role = new Role { Name = roleName };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new Exception($"Error creating role: {string.Join(", ", result.Errors)}");
            }
        }

        public async Task Delete(string roleName)
        {
            var role = await roleManager
                .FindByNameAsync(roleName) ?? throw new HttpException(ErrorMessages.RoleByNameNotFound, HttpStatusCode.BadRequest);

            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);

            foreach (var user in usersInRole)
            {
                await userManager.RemoveFromRoleAsync(user, role.Name);
            }

            await roleManager.DeleteAsync(role);
        }

        public async Task AddToRole(string userId, string roleName)
        {
            var user = await userManager
                .FindByIdAsync(userId) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            _ = await roleManager
                .FindByNameAsync(roleName) ?? throw new HttpException(ErrorMessages.RoleByNameNotFound, HttpStatusCode.BadRequest);

            await userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveFromRole(string userId, string roleName)
        {
            var user = await userManager
                .FindByIdAsync(userId) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            _ = await roleManager
                .FindByNameAsync(roleName) ?? throw new HttpException(ErrorMessages.RoleByNameNotFound, HttpStatusCode.BadRequest);

            await userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var roles = await roleManager.Roles.ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string userId)
        {
            var user = await userManager
                .FindByIdAsync(userId) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
           
            var roles = await userManager.GetRolesAsync(user);

            return roles;
        }
    }
}
