

using Microsoft.AspNetCore.Hosting;
using QRCoder;
using System.Drawing.Imaging;

namespace Infrastructure.Services;

public static class QR
{
    public static string Generate(int userId, IWebHostEnvironment webHostEnvironment)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode($"{userId}", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);

        var qrCodeImage = qrCode.GetGraphic(20);

        var wwwrootPath = webHostEnvironment.WebRootPath;
        var qrCodeFilePath = Path.Combine(wwwrootPath, "qrcodes", $"{userId}.png");

        using (var stream = new FileStream(qrCodeFilePath, FileMode.Create))
        {
            qrCodeImage.Save(stream, ImageFormat.Png);
        }
        return $"{userId}.png";
    }
}
