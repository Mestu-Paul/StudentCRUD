using A.Contracts.DataTransferObjects;
using A.Contracts.Entities;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IAccountDataAccess
    {
        Task<List<UserDTO>> GetUsersAsync();

        Task<User> GetUserAsync(string username);

        Task CreateNewUser(string username, string password, string role);

        Task<User> Login(string username, string password);

        Task UpdateUserRole(string username, string newRole);

        Task<bool> DeleteUserAsync(string username);
    }
}
