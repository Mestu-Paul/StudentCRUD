using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A.Contracts.Models;

namespace B.DatabaseAccess.IDataAccess
{
    public interface ISharedDataAccess
    {
        public Task<bool> DeleteUserAsync(string username);
        public Task CreateNewUser(string username, string role);
        public Task<UserRecords> GetUserRecords();
    }
}
