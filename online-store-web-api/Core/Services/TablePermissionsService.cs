using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Resources;
using Core.Specification;
using System.Net;
using Core.Models.Permission;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class TablePermissionsService(IRepository<TablePermission> tablePermissionsRepo,
                                         UserManager<User> userManager,
                                         IModeratingTableService moderatingTableService,
                                         IMapper mapper) : ITablePermissionsService
    {
        public async Task<IEnumerable<GetTablePermissionDto>> GetAll()
        {
            var permissions = await tablePermissionsRepo.GetAllBySpec(new Permissions.All());

            return mapper.Map<IEnumerable<GetTablePermissionDto>>(permissions);
        }

        public async Task<GetTablePermissionDto> GetById(int id)
        {
            var permission = await tablePermissionsRepo
                .GetBySpec(new Permissions.ById(id)) ?? throw new HttpException(ErrorMessages.PermissionByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetTablePermissionDto>(permission);
        }

        public async Task<IEnumerable<GetTablePermissionDto>> GetByUserId(string userId)
        {
            var permissions = await tablePermissionsRepo
                .GetAllBySpec(new Permissions.ByUserId(userId)) ?? throw new HttpException(ErrorMessages.PermissionByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<IEnumerable<GetTablePermissionDto>>(permissions);
        }

        public async Task Replace(string userId, List<int> moderatingTablesIds)
        {        
            var user = await userManager
                .FindByIdAsync(userId) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            if (await userManager.IsInRoleAsync(user, "Moderator"))
            {
                var existingPermissions = await tablePermissionsRepo.GetAllBySpec(new Permissions.ByUserId(userId));

                foreach (var permission in existingPermissions)
                {
                    await Delete(permission.Id);
                }

                foreach (var createPermissionDto in moderatingTablesIds.Select(moderatingTableId => new CreateTablePermissionDto
                {
                    UserId = userId,
                    ModeratingTableId = moderatingTableId,
                }))
                {
                    await Create(createPermissionDto);
                }
            }
            else
            {
                throw new HttpException(ErrorMessages.UserDoesNotHaveModeratorRole, HttpStatusCode.NotFound);
            }
        }

        public async Task Create(CreateTablePermissionDto createPermissionDto)
        {
            var user = await userManager
                .FindByIdAsync(createPermissionDto.UserId) ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            if (await userManager.IsInRoleAsync(user, "Moderator"))
            {
                var permissionEntity = mapper.Map<TablePermission>(createPermissionDto);
                await tablePermissionsRepo.Insert(permissionEntity);
                await tablePermissionsRepo.Save();
            }
            else
            {
                throw new HttpException(ErrorMessages.UserDoesNotHaveModeratorRole, HttpStatusCode.NotFound);
            }
        }

        public async Task ClearByUserId(string userId)
        {
            var permissions = await tablePermissionsRepo.GetAllBySpec(new Permissions.ByUserId(userId));

            foreach (var permission in permissions)
            {
                await Delete(permission.Id);
            }
        }

        public async Task Delete(int id)
        {
            var existingPermission = await tablePermissionsRepo
                .GetBySpec(new Permissions.ById(id)) ?? throw new HttpException(ErrorMessages.PermissionByIdNotFound, HttpStatusCode.NotFound);

            await tablePermissionsRepo.Delete(existingPermission);
            await tablePermissionsRepo.Save();
        }

        public async Task<bool> CanModifyTable(string userId, int moderatingTableId)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound); ;

            if (!await userManager.IsInRoleAsync(user, "Moderator"))
            {
                return false;
            }

            var permissions = await tablePermissionsRepo.GetAllBySpec(new Permissions.ByUserId(userId));
            return permissions.FirstOrDefault(mp => mp.ModeratingTableId == moderatingTableId) != null;
        }

        public async Task<bool> CanModifyTable(string userId, string moderatingTableName)
        {
            var user = await userManager.FindByIdAsync(userId)
                       ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var moderatingTable = await moderatingTableService.GetByName(moderatingTableName);

            if (!await userManager.IsInRoleAsync(user, "Moderator"))
            {
                return false;
            }

            var permissions = await tablePermissionsRepo.GetAllBySpec(new Permissions.ByUserId(userId));
            return permissions.FirstOrDefault(mp => mp.ModeratingTableId == moderatingTable.Id) != null;
        }
    }
}
