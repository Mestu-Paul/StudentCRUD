using _1.BOL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._DAL.IDataServices
{
    public interface IStudentDAL
    {
        Task CreateAsync(Students student);
        Task<List<Students>> GetAsync();
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(string id, Students student);
        Task<bool> PatchAsync(string id, JsonPatchDocument<Students> patchDocument);
        Task<List<Students>> GetStudentsPageAsync(int pageNumber, int pageSize);
    }
}
