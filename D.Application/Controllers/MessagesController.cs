using System.Net.WebSockets;
using A.Contracts.DataTransferObjects;
using C.BusinessLogic.ILoigcs;
using D.Application.WebSocket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageLogic _messageLogic;
        private readonly WebSocketHandler _webSocketHandler;

        public MessagesController(IMessageLogic messageLogic, WebSocketHandler webSocketHandler)
        {
            _messageLogic = messageLogic;
            _webSocketHandler = webSocketHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessage(string senderUsername, string receiverUsername, int pagenumber)
        {
            if (senderUsername == receiverUsername) return BadRequest("Sender and recipient name can not be same");
            string username = HttpContext.Items["Username"] as string;
            if (!(username==senderUsername || username==receiverUsername)) return BadRequest("Invalid request");

            return Ok(await _messageLogic.GetMessageListAsync(senderUsername, receiverUsername, pagenumber));
        }

        [Route("chatList")]
        [HttpGet]
        public async Task<IActionResult> GetChatList(string username)
        {
            if (username == null)
            {
                username = HttpContext.Items["Username"] as string;
            }
            return Ok(await _messageLogic.GetChatList(username));
        }


        [Route("searchUsers")]
        [HttpGet]
        public async Task<IActionResult> GetSearchUsers([FromQuery] string? username)
        {
            int pageNumber = 1, pageSize = 10;
            return Ok(await _messageLogic.GetSearchUsersAsync(username, pageNumber, pageSize));
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDTO messageDto)
        {
            if (string.IsNullOrEmpty(messageDto.RecipientUsername) || string.IsNullOrEmpty(messageDto.SenderUsername) ||
                string.IsNullOrEmpty(messageDto.Content))
            {
                return BadRequest("Invalid information");
            }
            try
            {
                await _messageLogic.SendMessage(messageDto);
                await _webSocketHandler.SendMessageToUser(messageDto);
                return Created();
            }
            catch (WebSocketException e)
            {
                await _webSocketHandler.RemoveConnection(messageDto.RecipientUsername);
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("newMessage")]
        public async Task<IActionResult> GetUnreadMessageCountAsync(string? username)
        {
            if (username == null)
            {
                username = HttpContext.Items["Username"] as string;
            }
            return Ok(await _messageLogic.GetUnreadMessageCountAsync(username));
        }

    }
}
