using Microsoft.AspNetCore.Http;

namespace Core.Models.Banner
{
    public class UpdateBannerDto
    {
        public IFormFile Image { get; set; }
    }
}
