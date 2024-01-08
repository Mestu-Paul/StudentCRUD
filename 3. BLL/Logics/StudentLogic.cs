using _1.BOL.Entities;
using _2._DAL.IDataServices;
using _3._BLL.ILogics;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._BLL.Logics
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IStudentDAL _studentDAL;

        public StudentLogic(IStudentDAL studentDAL)
        {
            _studentDAL = studentDAL;
        }

        public async Task CreateStudentAsync(Students student)
        {
            await _studentDAL.CreateAsync(student);
            return;
        }

        public async Task<List<Students>> GetAllStudentsAsync()
        {
            return await _studentDAL.GetAsync();
        }

        public async Task<bool> DeleteStudentAsync(string id)
        {
            // Additional logic or validation before deletion can be added here
            return await _studentDAL.DeleteAsync(id);
        }

        public async Task<bool> UpdateStudentAsync(string id, Students student)
        {
            return await _studentDAL.UpdateAsync(id, student);
        }

        public async Task<bool> PatchStudentAsync(string id, JsonPatchDocument<Students> patchDocument)
        {
            // Additional logic or validation before patch can be added here
            return await _studentDAL.PatchAsync(id, patchDocument);
        }

        public async Task<List<Students>> GetStudentsPageAsync(int pageNumber, int pageSize)
        {
            return await _studentDAL.GetStudentsPageAsync(pageNumber, pageSize);
        }

        public async Task<long> GetNumberOfDataAsync()
        {
            return await _studentDAL.NumberOfDataAsync();
        }
    }
}
