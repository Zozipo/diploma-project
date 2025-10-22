using Microsoft.AspNetCore.Http;

namespace Core.Models.ModeratingTable
{
    public class CreateModeratingTableDto
    {
        public string TableName { get; set; }
        public IFormFile Image { get; set; }
    }
}
