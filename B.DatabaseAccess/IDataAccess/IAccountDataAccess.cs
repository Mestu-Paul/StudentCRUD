﻿using A.Contracts.Models;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IAccountDataAccess
    {
        Task<List<User>> GetUsersAsync();
        Task CreateNewUser(string username, string password, string role);
        Task<User> Login(string username, string password);
        Task<bool> DeleteUser(string username);
        Task UpdateUserRole(string username, string newRole);
    }
}
