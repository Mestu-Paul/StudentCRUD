using A.Contracts.DataTransferObjects;
using A.Contracts.DBSettings;
using A.Contracts.Entities;
using B.DatabaseAccess.IDataAccess;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace B.DatabaseAccess.DataAccess
{
    public class MessageDataAccess : IMessageDataAccess
    {
        private readonly IMongoCollection<Message> _messageCollection;
        private readonly IMongoCollection<ChatList> _chatListCollection;

        public MessageDataAccess(IOptions<MongoDBSetting> mongoDbSetting)
        {
            string collectionName = "message";
            MongoClient client = new MongoClient(mongoDbSetting.Value.ConnectionStrings);
            IMongoDatabase database = client.GetDatabase(mongoDbSetting.Value.DatabaseName);
            _messageCollection = database.GetCollection<Message>(collectionName);
            _chatListCollection = database.GetCollection<ChatList>("chatList");
        }

        public async Task<List<MessageDTO>> GetMessageListAsync(string senderUsername, string receiverUsername, int pagenumber)
        {
            int pagesize = 20;
            int skipCount = (pagenumber - 1) * pagesize;
            var filterBuilder = Builders<Message>.Filter;
            var filter = filterBuilder.And(
                filterBuilder.Or(
                    filterBuilder.Eq(m => m.SenderUsername, senderUsername),
                    filterBuilder.Eq(m => m.Recipientname, senderUsername)
                ),
                filterBuilder.Or(
                    filterBuilder.Eq(m => m.SenderUsername, receiverUsername),
                    filterBuilder.Eq(m => m.Recipientname, receiverUsername)
                )
            );

            // Retrieve messages from the collection
            var messages = await _messageCollection.Find(filter)
                .Skip(skipCount)
                .Limit(pagesize)
                .ToListAsync();

            // Convert messages to DTOs and return
            var messageDTOs = messages.Select(message => new MessageDTO
            {
                Id = message.Id,
                SenderUsername = message.SenderUsername,
                Recipientname = message.Recipientname,
                Content =  message.Content,
                MessageSent = message.MessageSent
            }).ToList();

            return messageDTOs;
        }

        public async Task SendMessage(MessageDTO messageDTO)
        {
            Message message = new Message(messageDTO);
            await _messageCollection.InsertOneAsync(message);
            return;
        }

        public async Task<ChatList> GetChatList(string username)
        {
            var filter = Builders<ChatList>.Filter.Eq(u => u.username, username);
            var sortDefinition = Builders<ChatList>.Sort.Ascending("recipient.timestamp");

            var chatList = await _chatListCollection.Find(filter)
                .Sort(sortDefinition)
                .FirstOrDefaultAsync();

            return chatList;
        }

        public async Task AddOrUpdateChatList(string sender, string recipient)
        {
            var existingChatList = await _chatListCollection.Find(u => u.username == sender).FirstOrDefaultAsync();

            if (existingChatList != null)
            {
                var existance = existingChatList.recipientUsername.Find(u => u.username == recipient);
                if (existance == null)
                {
                    existingChatList.recipientUsername.Insert(0, new Recipient
                        { username = recipient, timestamp = DateTime.Now });
                }
                else
                {
                    existance.timestamp = DateTime.Now;
                }
                await _chatListCollection.ReplaceOneAsync(u => u.username == sender, existingChatList);
            }
            else
            {
                var chatList = new ChatList
                {
                    username = sender,
                    recipientUsername = new List<Recipient> { new Recipient { username = recipient, timestamp = DateTime.Now } }
                };

                await _chatListCollection.InsertOneAsync(chatList);
            }
        }

    }
}

