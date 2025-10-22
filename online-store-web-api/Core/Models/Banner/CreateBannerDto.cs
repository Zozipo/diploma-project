using Microsoft.AspNetCore.Http;

namespace Core.Models.Banner
{
    public class CreateBannerDto
    {
        public IFormFile Image { get; set; }
    }
}
