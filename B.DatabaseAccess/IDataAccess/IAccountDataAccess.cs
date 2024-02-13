using A.Contracts.DataTransferObjects;
using A.Contracts.Entities;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IAccountDataAccess
    {
        Task<List<UserDTO>> GetUsersAsync();

        Task<List<UserDTO>> GetSearchUsers(string? username, int pageNumber = 1, int pageSize=20);

        Task<User> GetUserAsync(string username);

        Task CreateNewUser(string username, string password, string role);

        Task<User> Login(string username, string password);

        Task UpdateUserRole(string username, string newRole);

        Task<bool> DeleteUserAsync(string username);

        Task<List<UserDTO>> GetFilteredUsersAsync(int pageNumber);
        Task<long> GetUsersCount();
    }
}
