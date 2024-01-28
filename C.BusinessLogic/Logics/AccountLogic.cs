using A.Contracts.DataTransferObjects;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using System.Security.Cryptography;
using System.Text;

namespace C.BusinessLogic.Logics
{
    public class AccountLogic : IAccountLogic
    {
        private readonly IAccountDataAccess _accountDataAccess;

        public AccountLogic(IAccountDataAccess accountDataAccess)
        {
            _accountDataAccess = accountDataAccess;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _accountDataAccess.GetUsersAsync();
        }

        public async Task CreateUser(string username, string password, string role)
        {
            await _accountDataAccess.CreateNewUser(username, password, role.ToLower());
            
            return;
        }

        public async Task UpdateUserRole(string username, string newRole)
        {
            await _accountDataAccess.UpdateUserRole(username, newRole.ToLower());
            return;
        }

        public async Task<bool> DeleteUser(string username)
        {
            return await _accountDataAccess.DeleteUser(username);
        }

        public async Task<User> Login(string username, string password)
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

            return user;
        }
    }
}
