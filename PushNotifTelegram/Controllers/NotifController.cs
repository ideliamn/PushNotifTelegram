using Microsoft.AspNetCore.Mvc;
using PushNotifTelegram.Services;

namespace PushNotifTelegram.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class NotifController : Controller
    {
        private readonly TelegramServices _telegramServices;
        public NotifController(TelegramServices telegramServices)
        {
            _telegramServices = telegramServices;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotif([FromBody] string message)
        {
            await _telegramServices.SendMessage(message);
            return Ok("Success send message");
        }
    }
}
