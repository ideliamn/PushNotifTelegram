using Microsoft.AspNetCore.Mvc;
using OpenTl.Schema.Messages;
using PushNotifTelegram.Models;
using PushNotifTelegram.Services;
using TL;
using static System.Net.Mime.MediaTypeNames;

namespace PushNotifTelegram.Controllers
{
    [ApiController]
    [Route("/api/telegram")]
    public class TelegramController : Controller
    {
        private readonly TelegramServices _telegramService;

        public TelegramController(TelegramServices telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] Models.RequestSendMessage param)
        {
            await _telegramService.InitAsync();
            if (param.username == null && param.phone_number == null)
            {
                return BadRequest("Masukkan username / nomor telepon!");
            }
            if (param.username != null)
            {
                await _telegramService.SendMessageToUsernameAsync(param.username, param.message);
            }
            if (param.phone_number != null)
            {
                await _telegramService.SendMessageToPhoneAsync(param.phone_number, param.message);
            }
            return Ok("Pesan terkirim");
        }
    }
}
