namespace A.Contracts.Models
{
    internal class MessageModel
    {
    }

    public class SenderUser
    {
        public string Username { get; set; }
        public long UnreadMessageCount { get; set; }
    }
}
