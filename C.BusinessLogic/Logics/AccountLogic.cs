using A.Contracts.DataTransferObjects;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using System.Security.Cryptography;
using System.Text;
using C.BusinessLogic.Services;
using A.Contracts.Entities;
using A.Contracts.Models;

namespace C.BusinessLogic.Logics
{
    public class AccountLogic : IAccountLogic
    {
        private readonly IAccountDataAccess _accountDataAccess;
        private readonly ITokenService _tokenService;
        private readonly ISharedDataAccess _sharedDataAccess;

        public AccountLogic(IAccountDataAccess accountDataAccess, ITokenService tokenService, ISharedDataAccess sharedDataAccess)
        {
            _accountDataAccess = accountDataAccess;
            _tokenService = tokenService;
            _sharedDataAccess = sharedDataAccess;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            return await _accountDataAccess.GetUsersAsync();
        }

        public async Task<Tuple<List<UserDTO>, long>> GetFilteredUsersAsync(int pageNumber)
        {
            int pageSize = 20;
            List<UserDTO> students = await _accountDataAccess.GetFilteredUsersAsync(pageNumber);
            long count = await _accountDataAccess.GetUsersCount();

            count = (count + pageSize - 1) / pageSize;

            return Tuple.Create(students, count);
        }

        public async Task CreateUser(string username, string password, string role)
        {
            await _accountDataAccess.CreateNewUser(username.ToLower(), password, role.ToLower());
            await _sharedDataAccess.CreateNewUser(username.ToLower(),role.ToLower());
            return;
        }

        public async Task UpdateUserRole(string username, string newRole)
        {
            await _accountDataAccess.UpdateUserRole(username, newRole.ToLower());
            return;
        }

        public async Task<bool> DeleteUser(string username)
        {
            return await _sharedDataAccess.DeleteUserAsync(username);
        }

        public async Task<String> Login(string username, string password)
        {
            User user = await _accountDataAccess.Login(username, password);
            if (user == null)
            {
                throw new Exception("Invalid username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != user.PasswordHash[i])
                    throw new Exception("Invalid password");
            }

            return _tokenService.CreateToken(user);
        }
    }
}
