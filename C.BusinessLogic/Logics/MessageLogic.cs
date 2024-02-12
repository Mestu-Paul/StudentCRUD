using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A.Contracts.DataTransferObjects;
using A.Contracts.Entities;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;

namespace C.BusinessLogic.Logics
{
    public class MessageLogic : IMessageLogic
    {
        private readonly IMessageDataAccess _messageDataAccess;
        public MessageLogic(IMessageDataAccess messageDataAccess)
        {
            _messageDataAccess = messageDataAccess;
        }


        public async Task<List<MessageDTO>> GetMessageListAsync(string senderUsername, string receiverUsername, int pagenumber)
        {
            return await _messageDataAccess.GetMessageListAsync(senderUsername, receiverUsername, pagenumber);
        }

        public async Task SendMessage(MessageDTO messageDTO)
        {
            await _messageDataAccess.SendMessage(messageDTO);
            await _messageDataAccess.AddOrUpdateChatList(messageDTO.SenderUsername, messageDTO.Recipientname);
            await _messageDataAccess.AddOrUpdateChatList(messageDTO.Recipientname, messageDTO.SenderUsername);
            return;
        }

        public async Task<ChatList> GetChatList(string username)
        {
            return await _messageDataAccess.GetChatList(username);
        }
    }
}
