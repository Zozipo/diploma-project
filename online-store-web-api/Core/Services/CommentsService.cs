using Core.Interfaces;
using Core.Entities;
using AutoMapper;
using Core.Helpers;
using System.Net;
using Core.Resources;
using Core.Models.Comment;
using Core.Specification;

namespace Core.Services
{
    public class CommentsService(IRepository<Comment> commentsRepo, IRepository<CommentImage> commentsImageRepo,
                                 IMapper mapper, IImagesService imagesService) : ICommentsService
    {
        public async Task<IEnumerable<GetCommentDto>> GetAll()
        {
            var comments = await commentsRepo.GetAllBySpec(new Comments.All());
            return mapper.Map<IEnumerable<GetCommentDto>>(comments);
        }

        public async Task<GetCommentDto?> GetById(int id)
        {
            Comment comment = await commentsRepo
                .GetBySpec(new Comments.ById(id)) ?? throw new HttpException(ErrorMessages.CommentByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetCommentDto>(comment);
        }

        public async Task<IEnumerable<GetCommentDto>> GetByProductId(int productId)
        {
            var comments = await commentsRepo.GetAllBySpec(new Comments.ByProductId(productId));
            return mapper.Map<IEnumerable<GetCommentDto>>(comments);
        }

        public async Task<IEnumerable<GetCommentDto>> GetByUserId(string userId)
        {
            var comments = await commentsRepo.GetAllBySpec(new Comments.ByUserId(userId));
            return mapper.Map<IEnumerable<GetCommentDto>>(comments);
        }

        public async Task Create(CreateCommentDto createCommentDto)
        {
            var newComment = mapper.Map<Comment>(createCommentDto);

            if (createCommentDto.Images != null && createCommentDto.Images.Any())
            {
                var imageEntities = new List<CommentImage>();
                foreach (var imageFile in createCommentDto.Images)
                {
                    var imageEntity = new CommentImage
                    {
                        Image = imagesService.SaveImage(imageFile),
                        Comment = newComment
                    };
                    imageEntities.Add(imageEntity);
                }

                newComment.Images = imageEntities;

                await commentsRepo.Insert(newComment);
                await commentsRepo.Save();

            }
        }

        public async Task Update(int commentId, UpdateCommentDto updateCommentDto)
        {
            var existingComment = await commentsRepo.GetByID(commentId);

            if (existingComment == null)
                throw new HttpException(ErrorMessages.CommentByIdNotFound, HttpStatusCode.NotFound);

            if (updateCommentDto.Images != null && updateCommentDto.Images.Any())
            {
                foreach (var imageFile in updateCommentDto.Images)
                {
                    var commentImage = new CommentImage
                    {
                        Image = imagesService.SaveImage(imageFile),
                        CommentId = commentId 
                    };

                    await commentsImageRepo.Insert(commentImage);
                }
            }

            mapper.Map(updateCommentDto, existingComment);

            await commentsRepo.Update(existingComment); 
            await commentsRepo.Save();
        }


        public async Task Delete(int id)
        {
            var existingProduct = await commentsRepo.GetByID(id) ??
                                 throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            if (existingProduct.Images != null && existingProduct.Images.Any())
            {
                foreach (var image in existingProduct.Images)
                {
                    imagesService.DeleteImage(image.Image);

                    await commentsImageRepo.Delete(image.Id);
                    await commentsImageRepo.Save();

                }
            }

            await commentsRepo.Delete(id);
            await commentsRepo.Save();
        }
    }
}
