namespace A.Contracts.Models
{
    internal class MessageModel
    {
    }

    public class SenderUser
    {
        public string username { get; set; }
        public long UnreadMessageCount { get; set; }
    }

    public class UnreadMessage
    {
        public HashSet<List<SenderUser>> Users { get; set; }
        public long UnreadMessageCount { get; set; }
    }
}
