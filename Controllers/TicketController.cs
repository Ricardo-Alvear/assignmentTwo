using comp2147.data;
using comp2147.Services.QrCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace comp2147.Controllers;

public class TicketController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly QrCodeService _qr;

    public TicketController(ApplicationDbContext db, QrCodeService qr)
    {
        _db = db;
        _qr = qr;
    }

    [HttpGet]
    public async Task<IActionResult> Qr(int id)
    {
        var ticket = await _db.OrderItems
            .Include(o => o.Event)
            .Include(o => o.Order).ThenInclude(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (ticket == null) return NotFound();

        string qrPayload = $"Ticket:{ticket.Id}|Event:{ticket.Event.Title}|User:{ticket.Order.User.Email}";
        var qrBytes = _qr.GenerateQrBytes(qrPayload);
        return File(qrBytes, "image/png");
    }

    private byte[] GenerateQrBytes(string text)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var pngQr = new PngByteQRCode(qrCodeData);
        return pngQr.GetGraphic(20);
    }
    
}