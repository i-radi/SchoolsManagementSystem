using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public static class Picture
{
    public static async Task<string> Upload(IFormFile profilePicture, IWebHostEnvironment webHostEnvironment)
    {
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profilePicture.FileName);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await profilePicture.CopyToAsync(fileStream);
        }
        return uniqueFileName;
    }
}
