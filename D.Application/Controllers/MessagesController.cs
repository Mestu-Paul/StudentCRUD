using A.Contracts.DataTransferObjects;
using C.BusinessLogic.ILoigcs;
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
        public MessagesController(IMessageLogic messageLogic)
        {
            _messageLogic = messageLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessage(string senderUsername, string receiverUsername, int pagenumber)
        {
            return Ok(await _messageLogic.GetMessageListAsync(senderUsername, receiverUsername, pagenumber));
        }

        [Route("chatList")]
        [HttpGet]
        public async Task<IActionResult> GetChatList(string username)
        {
            //if (username == null)
            //{
            //    username = HttpContext.Items["Username"] as string;
            //}
            return Ok(await _messageLogic.GetChatList(username));
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDTO messageDto)
        {
            try
            {
                await _messageLogic.SendMessage(messageDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
