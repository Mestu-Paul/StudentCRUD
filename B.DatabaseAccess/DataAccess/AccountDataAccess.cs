﻿using A.Contracts.DBSettings;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<List<User>> GetUsersAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateNewUser(string username, string password, string role)
        {
            username = username.ToLower();
            
            var existingUser = await _userCollection.Find(u => u.UserName == username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new ArgumentException("Username is already taken", nameof(username));
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

        public async Task<bool> DeleteUser(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, username);
            var deleteResult = await _userCollection.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;
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
    }
}
