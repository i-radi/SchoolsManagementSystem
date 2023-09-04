using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public static class Picture
{
    public static async Task<string> Upload(IFormFile profilePicture, IWebHostEnvironment webHostEnvironment, string picturePath, string filename)
    {
        string filePath = Path.Combine(webHostEnvironment.WebRootPath, picturePath, filename);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await profilePicture.CopyToAsync(fileStream);
        }
        return filename;
    }
}
