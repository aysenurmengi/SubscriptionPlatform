using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Interfaces;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-test-email")]
        public async Task<IActionResult> SendTestEmail([FromQuery] string targetEmail)
        {
            try
            {
                string subject = "Abonelik Platformu | Test Maili";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; border: 1px solid #eee; padding: 20px;'>
                        <h2 style='color: #2e7d32;'>Sistem Çalışıyor!</h2>
                        <p>Bu mail, <b>EmailService</b> testi için gönderilmiştir.</p>
                        <p>Gönderim saati: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                        <hr>
                        <p style='font-size: 12px; color: #777;'>Subscription Platform Otomasyon Testi</p>
                    </div>";

                await _emailService.SendEmailAsync(targetEmail, subject, body);

                return Ok(new { message = "Test maili Mailtrap'e gönderildi. Lütfen Mailtrap panelini kontrol edin!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, detail = ex.InnerException?.Message });
            }
        }
    }
}