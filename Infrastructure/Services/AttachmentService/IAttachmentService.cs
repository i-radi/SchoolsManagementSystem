using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public interface IAttachmentService
{
    Task<string> Upload(IFormFile file, IWebHostEnvironment webHostEnvironment, string path, string filename);
    string GenerateQrCode(int userId, IWebHostEnvironment webHostEnvironment);
}
