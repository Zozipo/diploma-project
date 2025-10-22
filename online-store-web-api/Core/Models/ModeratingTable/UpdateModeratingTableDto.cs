using Microsoft.AspNetCore.Http;

namespace Core.Models.ModeratingTable
{
    public class UpdateModeratingTableDto
    {
        public string? TableName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
