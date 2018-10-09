using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tincan.Cryptography;
using Tincan.Web.Controllers.Models;

namespace Tincan.Web.Controllers
{
    [ApiController]
    [Route("api/message")]
    [Produces("application/json")]
    public class MessageController : ControllerBase
    {
        private readonly IRepository _repository;

        public MessageController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageDto>> GetMessage(string id)
        {
            var message = await _repository.GetMessage(id);
            if (message == null || message.ExpiresAt < DateTime.UtcNow)
                return NotFound();

            return new MessageDto
            {
                ExpiresAt = message.ExpiresAt
            };
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateMessage(CreateMessageDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repository.CreateMessage(request.Key, request.Content, request.ExpiresAt);
            if (result == null)
                return BadRequest();

            return result.Id;
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> DecryptMessage(string id, DecryptMessageDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await Task.Delay(1000);

            var message = await _repository.GetMessage(id);
            if (message == null || message.ExpiresAt < DateTime.UtcNow)
                return BadRequest();

            if (Encryption.TryDecrypt(message.EncryptedContent, request.Key, out var content))
                return content;

            return BadRequest();
        }
    }
}