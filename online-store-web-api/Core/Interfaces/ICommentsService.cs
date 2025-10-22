using Core.Models.Comment;

namespace Core.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<GetCommentDto>> GetAll();
        Task<GetCommentDto?> GetById(int id);
        Task<IEnumerable<GetCommentDto>> GetByProductId(int productId);
        Task<IEnumerable<GetCommentDto>> GetByUserId(string userId);
        Task Create(CreateCommentDto category);
        Task Update(int commentId, UpdateCommentDto updateCommentDto);
        Task Delete(int id);
    }
}
