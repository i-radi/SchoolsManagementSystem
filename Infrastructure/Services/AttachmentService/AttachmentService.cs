using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QRCoder;
using System.Drawing.Imaging;

namespace Infrastructure.Services;

public class AttachmentService : IAttachmentService
{
    public async Task<string> Upload(IFormFile file, IWebHostEnvironment webHostEnvironment, string path, string filename)
    {
        string filePath = Path.Combine(webHostEnvironment.WebRootPath, path, filename);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filename;
    }

    public string GenerateQrCode(int userId, IWebHostEnvironment webHostEnvironment)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode($"{userId}", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);

        var qrCodeImage = qrCode.GetGraphic(20);

        var wwwrootPath = webHostEnvironment.WebRootPath;
        var qrCodeFilePath = Path.Combine(wwwrootPath, "uploads", "qrcodes", $"{userId}.png");

        using (var stream = new FileStream(qrCodeFilePath, FileMode.Create))
        {
            qrCodeImage.Save(stream, ImageFormat.Png);
        }
        return $"{userId}.png";
    }

}
