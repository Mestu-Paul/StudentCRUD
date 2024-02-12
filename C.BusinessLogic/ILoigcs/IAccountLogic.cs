using A.Contracts.DataTransferObjects;
using A.Contracts.Models;

namespace C.BusinessLogic.ILoigcs
{
    public interface IAccountLogic
    {
        Task<List<UserDTO>> GetUsersAsync();
        Task<Tuple<List<UserDTO>,long>> GetFilteredUsersAsync(int pageNumber);
        Task CreateUser(string username, string password, string role);
        Task UpdateUserRole(string username, string newRole);
        Task<bool> DeleteUser(string username);
        Task<String> Login(string username, string password);
    }
}
