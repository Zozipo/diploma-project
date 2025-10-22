using Core.Interfaces;
using Core.Models.Comment;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(ICommentsService commentsService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> Get()
        {
            return Ok(await commentsService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCommentDto>> GetById(int id)
        {
            return Ok(await commentsService.GetById(id));
        }

        [HttpGet("getByProductId/{productId}")]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> GetByProductId(int productId)
        {
            return Ok(await commentsService.GetByProductId(productId));
        }

        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> GetByUserId(string userId)
        {
            return Ok(await commentsService.GetByUserId(userId));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateCommentDto createCommentDto)
        {
            await commentsService.Create(createCommentDto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(int commentId, [FromForm] UpdateCommentDto editCommentDto)
        {
            await commentsService.Update(commentId, editCommentDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await commentsService.Delete(id);
            return Ok();
        }
    }
}
