using Microsoft.AspNetCore.Mvc;
using OpenTl.Schema.Messages;
using PushNotifTelegram.Exceptions;
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
            _telegramService.InitAsync().GetAwaiter().GetResult();
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] Models.RequestSendMessage param)
        {
            if (string.IsNullOrWhiteSpace(param.username) && string.IsNullOrWhiteSpace(param.phone_number))
            {
                return BadRequest(new ResponseSendMessage { status = "error", message = "Masukkan username atau nomor telepon!" });
            }

            try
            {
                if (param.username != null && !string.IsNullOrWhiteSpace(param.username))
                {
                    await _telegramService.SendMessageToUsernameAsync(param.username, param.message);
                }
                if (param.phone_number != null && !string.IsNullOrWhiteSpace(param.phone_number))
                {
                    await _telegramService.SendMessageToPhoneAsync(param.phone_number, param.message);
                }
                return Ok(new ResponseSendMessage { status = "success", message = "Pesan terkirim" });
            }
            catch (CustomException.BadRequestException ex)
            {
                return BadRequest(new ResponseSendMessage { status = "error", message = ex.Message });
            }
            catch (CustomException.NotConnectedException ex)
            {
                return StatusCode(503, new ResponseSendMessage { status = "error", message = ex.Message });
            }
            catch (CustomException.UserNotFoundException ex)
            {
                return NotFound(new ResponseSendMessage { status = "error", message = ex.Message });
            }
            catch (CustomException.TooManyRequestsException ex)
            {
                return StatusCode(429, new ResponseSendMessage { status = "error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseSendMessage { status = "error", message = "Terjadi kesalahan pada server: " });
            }
        }
    }
}
