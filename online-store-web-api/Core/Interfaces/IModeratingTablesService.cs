using Core.Models.ModeratingTable;

namespace Core.Interfaces
{
    public interface IModeratingTableService
    {
        Task<IEnumerable<GetModeratingTableDto>> GetAll();
        Task<GetModeratingTableDto> GetById(int id);
        Task<GetModeratingTableDto> GetByName(string moderatingTableName);
        Task Create(CreateModeratingTableDto createModeratingTableDto);
        Task Update(int id, UpdateModeratingTableDto updateModeratingTableDto);
        Task Delete(int id);
    }
}
