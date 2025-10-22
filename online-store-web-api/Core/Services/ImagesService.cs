using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Core.Services
{
    public class ImageService : IImagesService
    {
        public string SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageFile), "Image file is required.");
            }

            string fileName = Path.GetRandomFileName() + ".webp";
            try
            {
                using (var stream = new MemoryStream())
                {
                    imageFile.CopyTo(stream);
                    byte[] byteBuffer = stream.ToArray();

                    SaveImageWithSize(byteBuffer, fileName, 150);
                    SaveImageWithSize(byteBuffer, fileName, 300);
                    SaveImageWithSize(byteBuffer, fileName, 600);
                    SaveImageWithSize(byteBuffer, fileName, 1200);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File could not be saved. " + ex.Message);
            }
            return fileName;
        }

        private void SaveImageWithSize(byte[] byteBuffer, string originalFileName, int size)
        {
            try
            {
                using (var image = Image.Load(byteBuffer))
                {
                    if (size > 0)
                    {
                        image.Mutate(x =>
                        {
                            x.Resize(new ResizeOptions
                            {
                                Size = new Size(size, size),
                                Mode = ResizeMode.Max
                            });
                        });
                    }

                    string fileName = (size > 0) ? $"{size}_{Path.GetFileNameWithoutExtension(originalFileName)}.webp" : originalFileName;
                    string dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, new WebpEncoder());
                        File.WriteAllBytes(dirSave, ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"File could not be saved with size {size}px. {ex.Message}");
            }
        }

        public void DeleteImage(string originalFileName)
        {
            if (originalFileName != "default_user.webp")
            {
                try
                {
                    string directory = Path.Combine(Directory.GetCurrentDirectory(), "images");

                    foreach (var size in new[] { 150, 300, 600, 1200 })
                    {
                        string fileNameToDelete = $"{size}_{Path.GetFileNameWithoutExtension(originalFileName)}.webp";
                        string fileToDelete = Path.Combine(directory, fileNameToDelete);

                        if (File.Exists(fileToDelete))
                        {
                            File.Delete(fileToDelete);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Files could not be deleted. " + ex.Message);
                }
            }
        }
    }
}
