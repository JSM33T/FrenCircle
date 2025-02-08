using FrenCircle.Entities.Data;
using FrenCircle.Helpers.Mappers;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace FrenCircle.Base.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : FcBaseController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ITelegramService _telegramService;
        private readonly IRateLimiter _rateLimiter;
        public MessageController(IMessageRepository messageRepository, IRateLimiter rateLimiter, ITelegramService telegramService)
        {
            _messageRepository = messageRepository;
            _telegramService = telegramService;
            _rateLimiter = rateLimiter;
        }
     

        [HttpPost("send")]
        public async Task<IActionResult> AddMessage(AddMessageRequest messageRequest)
        {
            if (_rateLimiter.IsRateLimited("GLOBAL", 5, 60))
            {
                return RESP_ForbiddenResponse("Daddy chill....");
            }

            var message = MessageDtoMappers.MAP_AddMessageRequest_Message(messageRequest);

            if (await _messageRepository.IsMessagePresent(message))
                return RESP_ConflictResponse("Message already present");

            _ = _telegramService.SendMessageAsync($" Message from {message.Name} || {message.Email} :\n {message.Text}");

            await _messageRepository.AddMessage(message);

            return RESP_Success("Message submitted");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllMessages() =>
            RESP_Success(await _messageRepository.GetAllMessages(), "Message submitted");
    }
}