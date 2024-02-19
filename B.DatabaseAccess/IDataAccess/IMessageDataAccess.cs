using A.Contracts.DataTransferObjects;
using A.Contracts.Entities;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IMessageDataAccess
    {
        Task<List<MessageDTO>> GetMessageListAsync(string senderUsername, string receiverUsername, int pagenumber);
        Task SendMessage(MessageDTO messageDTO);

        Task<ChatList> GetChatList(string username);

        Task AddOrUpdateChatList(string sender, string receipient);

        Task<long> GetUnreadMessageCountAsync(string username);
    }
}
