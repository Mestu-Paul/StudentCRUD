using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.Logics
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IStudentDataAccess _studentsService;

        public StudentLogic(IStudentDataAccess studentsService)
        {
            _studentsService = studentsService;
        }

        public async Task CreateNewStudentAsync(Student student)
        {
            await _studentsService.CreateNewStudentAsync(student);
            return;
        }
        
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentsService.GetAllStudentsAsync();
        }

        public async Task<List<Student>> GetStudentsPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid page number or size");
            }
            return await _studentsService.GetStudentsPagedAsync(pageNumber, pageSize);
        }

        public async Task<long> GetTotalNumberOfStudentsAsync()
        {
            return await _studentsService.GetTotalNumberOfStudentsAsync();
        }

        public async Task<bool> UpdateStudentSingleAttributeAsync(string id, JsonPatchDocument<Student> patchDocument)
        {
            
            return await _studentsService.UpdateStudentSingleAttributeAsync(id,  patchDocument); ;
        }

        public async Task<bool> UpdateStudentAsync(string id, Student student)
        {
            return await _studentsService.UpdateStudentAsync(id, student); ;
        }

        public async Task<bool> DeleteStudentAsync(string id)
        {
            return await _studentsService.DeleteStudentAsync(id); ;
        }
    }
}
