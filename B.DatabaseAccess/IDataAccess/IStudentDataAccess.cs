using A.Contracts.Entities;
using A.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IStudentDataAccess
    {
        Task CreateNewStudentAsync(Student student);

        Task<List<Student>> GetAllStudentsAsync();
        Task<long> GetStundentsCount();

        Task<Student> GetStudentAsync(string username);

        Task<Student> GetStudentByIdAsync(string id);

        Task<bool> UpdateStudentAsync(UpdateStudent student);

        Task<bool> PartialUpdateAsync(string username, JsonPatchDocument<Student> patchDocument);

        Task<List<Student>> GetFilteredStudentsAsync(StudentFilterParameters studentFilterParameters);

        Task<long> GetFilteredStudentsCountAsync(StudentFilterParameters studentFilterParameters);

        Task<bool> DeleteStudentAsync(string id);
    }
}
