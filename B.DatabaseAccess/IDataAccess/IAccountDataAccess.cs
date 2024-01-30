using A.Contracts.DataTransferObjects;
using A.Contracts.Models;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IAccountDataAccess
    {
        Task<List<UserDTO>> GetUsersAsync();
        Task CreateNewUser(string username, string password, string role);
        Task<User> Login(string username, string password);
        Task<bool> DeleteUser(string username);
        Task UpdateUserRole(string username, string newRole);
    }
}
