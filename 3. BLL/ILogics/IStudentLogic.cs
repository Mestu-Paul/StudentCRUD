using _1.BOL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._BLL.ILogics
{
    public interface IStudentLogic
    {
        Task CreateStudentAsync(Students student);
        Task<List<Students>> GetAllStudentsAsync();
        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(string id, Students student);
        Task<bool> PatchStudentAsync(string id, JsonPatchDocument<Students> patchDocument);
        Task<List<Students>> GetStudentsPageAsync(int pageNumber, int pageSize);
    }
}
