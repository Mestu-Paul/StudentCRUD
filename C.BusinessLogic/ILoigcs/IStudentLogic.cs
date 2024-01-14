using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.ILoigcs
{
    public interface IStudentLogic
    {
        Task CreateNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();
        Task<List<Student>> GetStudentsPagedAsync(int pageNumber, int pageSize);
        Task<long> GetTotalNumberOfStudentsAsync();
        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(string id, UpdateStudent student);
        Task<bool> UpdateStudentSingleAttributeAsync(string id, JsonPatchDocument<Student> patchDocument);
    }
}
