using A.Contracts.DBSettings;
using B.DatabaseAccess.IDataAccess;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using A.Contracts.DataTransferObjects;
using A.Contracts.Entities;
using A.Contracts.Models;

namespace B.DatabaseAccess.DataAccess
{
    public class AccountDataAccess : IAccountDataAccess
    {
        private readonly IMongoCollection<User> _userCollection;

        public AccountDataAccess(IOptions<MongoDBSetting> mongoDbSetting)
        {
            string collectionName = "user";
            MongoClient client = new MongoClient(mongoDbSetting.Value.ConnectionStrings);
            IMongoDatabase database = client.GetDatabase(mongoDbSetting.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(collectionName);

        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var projection = Builders<User>.Projection.Include(u => u.UserName).Include(u => u.Role);

            var users = await _userCollection.Find(new BsonDocument())
                .Project(projection)
                .ToListAsync();

            // Projecting the username and role properties into tuples
            List<UserDTO> result = users.Select(user => new UserDTO(user["userName"].AsString, user["role"].AsString)).ToList();
            return result;
        }

        public async Task<List<UserDTO>> GetSearchUsers(string? username, int pageNumber = 1, int pageSize=20)
        {
            var filter = Builders<User>.Filter.Empty;

            if(!string.IsNullOrEmpty(username))
                filter = Builders<User>.Filter.Regex(u => u.UserName, new BsonRegularExpression(username));

            var skipNumber = (pageNumber - 1) * pageSize;
            var users = await _userCollection.Find(filter)
                .Skip(skipNumber)
                .Limit(pageSize)
                .ToListAsync();

            List<UserDTO> result = users.Select(user => new UserDTO(user.UserName, user.Role)).ToList();
            return result;
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();
        }

        public async Task CreateNewUser(string username, string password, string role)
        {
            username = username.ToLower();
            
            var existingUser = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new ArgumentException("Username has already taken");
            }

            using var hmac = new HMACSHA512();
            var newUser = new User
            {
                UserName = username,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                Role = role
            };
            
            await _userCollection.InsertOneAsync(newUser);
        }

        public async Task<User> Login(string username, string password)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, username);
            return await _userCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task UpdateUserRole(string username, string newRole)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, username);
            var user = await _userCollection.Find(filter).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Role = newRole;
            await _userCollection.ReplaceOneAsync(filter, user);
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var result = await _userCollection.DeleteOneAsync(x => x.UserName == username);
            return result.DeletedCount > 0;
        }

        public async Task<long> GetUsersCount()
        {
            return await _userCollection.EstimatedDocumentCountAsync();
        }

        public async Task<List<UserDTO>> GetFilteredUsersAsync(int pageNumber)
        {
            var projection = Builders<User>.Projection.Include(u => u.UserName).Include(u => u.Role);
            int pageSize = 20;
            int skipCount = (pageNumber -1)* pageSize;
            var users = await _userCollection
                .Find(Builders<User>.Filter.Empty)
                .Project(projection)
                .Skip(skipCount)
                .Limit(pageSize)
                .ToListAsync();

            List<UserDTO> result = users.Select(user => new UserDTO(user["userName"].AsString, user["role"].AsString)).ToList();
            return result;
        }
    }
}
