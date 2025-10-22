using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IImagesService
    {
        string SaveImage(IFormFile imageFile);
        void DeleteImage(string fileName);
    }
}
