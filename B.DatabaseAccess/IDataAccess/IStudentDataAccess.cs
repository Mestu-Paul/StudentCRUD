using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IStudentDataAccess
    {
        Task CreateNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();
        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(string id, Student student);
        Task<bool> UpdateStudentSingleAttributeAsync(string id, JsonPatchDocument<Student> patchDocument);
        Task<List<Student>> GetStudentsPagedAsync(int pageNumber, int pageSize);
        Task<long> GetTotalNumberOfStudentsAsync();
    }
}
