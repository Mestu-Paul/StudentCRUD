using A.Contracts.Models;

namespace C.BusinessLogic.ILoigcs
{
    public interface IAccountLogic
    {
        Task<List<User>> GetUsersAsync();
        Task CreateUser(string username, string password, string role);
        Task UpdateUserRole(string username, string newRole);
        Task<bool> DeleteUser(string username);
        Task<User> Login(string username, string password);
    }
}
