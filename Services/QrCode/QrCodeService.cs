using QRCoder;

namespace comp2147.Services.QrCode
{
    public class QrCodeService
    {
        public byte[] GenerateQrBytes(string text, int pixelsPerModule = 20)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrCodeData);
            return pngQr.GetGraphic(pixelsPerModule);
        }
    }
}