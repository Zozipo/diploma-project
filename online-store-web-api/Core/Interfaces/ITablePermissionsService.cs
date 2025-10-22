using Core.Models.Permission;

namespace Core.Interfaces
{
    public interface ITablePermissionsService
    {
        Task<IEnumerable<GetTablePermissionDto>> GetAll();
        Task<GetTablePermissionDto> GetById(int id);
        Task<IEnumerable<GetTablePermissionDto>> GetByUserId(string userId);
        Task Replace(string userId, List<int> moderatingTablesIds);
        Task Create(CreateTablePermissionDto createPermissionDto);
        Task Delete(int id);
        Task ClearByUserId(string userId);
        Task<bool> CanModifyTable(string userId, int moderatingTableId);
        Task<bool> CanModifyTable(string userId, string moderatingTableName);
    }
}
