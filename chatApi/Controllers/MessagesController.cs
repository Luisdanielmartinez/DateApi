using AutoMapper;
using chatApi.DTOs;
using chatApi.Entities;
using chatApi.Extensions;
using chatApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        public readonly IMessageRepository _messageRepository;
        public readonly IMapper _mapper;
        public readonly IUserRepository _userRepository;
        public MessagesController(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessage)
        {
            var username = User.GetUserName();

            if (username == createMessage.RecipientUserName.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessage.RecipientUserName);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessage.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }
    }
}
