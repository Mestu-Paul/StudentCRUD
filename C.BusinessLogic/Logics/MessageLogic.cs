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
        private readonly IAccountDataAccess _accountDataAccess;

        public MessageLogic(IMessageDataAccess messageDataAccess, IAccountDataAccess accountDataAccess)
        {
            _messageDataAccess = messageDataAccess;
            _accountDataAccess = accountDataAccess;
        }


        public async Task<List<MessageDTO>> GetMessageListAsync(string senderUsername, string receiverUsername, int pagenumber)
        {
            return await _messageDataAccess.GetMessageListAsync(senderUsername, receiverUsername, pagenumber);
        }

        public async Task SendMessage(MessageDTO messageDTO)
        {
            await _messageDataAccess.SendMessage(messageDTO);
            await _messageDataAccess.AddOrUpdateChatList(messageDTO.SenderUsername, messageDTO.RecipientUsername);
            await _messageDataAccess.AddOrUpdateChatList(messageDTO.RecipientUsername, messageDTO.SenderUsername);
            return;
        }

        public async Task<ChatList> GetChatList(string username)
        {
            return await _messageDataAccess.GetChatList(username);
        }

        public async Task<List<UserDTO>> GetSearchUsersAsync(string? username, int pageNumber = 1, int pageSize = 20)
        {
            return await _accountDataAccess.GetSearchUsers(username, pageNumber, pageSize);
        }

        public async Task<long> GetUnreadMessageCountAsync(string username)
        {
            return await _messageDataAccess.GetUnreadMessageCountAsync(username);
        }
    }
}
