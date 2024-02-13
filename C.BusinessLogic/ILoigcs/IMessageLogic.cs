using A.Contracts.DataTransferObjects;
using A.Contracts.Entities;

namespace C.BusinessLogic.ILoigcs
{
    public interface IMessageLogic
    {
        Task<List<MessageDTO>> GetMessageListAsync(string senderUsername, string receiverUsername, int pagenumber);
        Task SendMessage(MessageDTO messageDTO);
        Task<ChatList> GetChatList(string username);

        Task<List<UserDTO>> GetSearchUsersAsync(string? username, int pageNumber = 1, int pageSize = 20);
    }
}
