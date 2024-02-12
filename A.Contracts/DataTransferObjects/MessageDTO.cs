namespace A.Contracts.DataTransferObjects
{
    public class MessageDTO
    {
        public string? Id { get; set; }
        public string SenderUsername { get; set; }
        public string Recipientname { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
