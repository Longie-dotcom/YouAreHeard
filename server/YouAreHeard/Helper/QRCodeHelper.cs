using QRCoder;
using System.IO;

public static class QRCodeHelper
{
    public static void SaveAppointmentQrCode(string data, string folderPath, string fileName)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);
        File.WriteAllBytes(filePath, qrCodeBytes);
    }
}
