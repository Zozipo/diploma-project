using Core.Models;
using Core.Models.Category;

namespace Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<GetCategoryDto>> GetAll();
        Task<GetCategoryDto?> GetById(int id);
        Task Create(CreateCategoryDto category);
        Task Update(int categoryId, UpdateCategoryDto updateCategoryDto);
        Task Delete(int id);
        Task<PaginatedList<GetCategoryDto>> GetByFilter(ItemsFilter itemsFilter);
        Task<GetCategorySelectionDto> GetByParentCategoryId(int parentId);
        Task<GetCategorySelectionDto> GetByParentCategoryName(string parentName);
        Task<IEnumerable<GetCategoryDto>> GetHead();
        Task<GetCategoryDto?> GetMostWatchedCategoryByUserId(string userId);
    }
}
