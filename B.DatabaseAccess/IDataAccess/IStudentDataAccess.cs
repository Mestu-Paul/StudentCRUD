using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace B.DatabaseAccess.IDataAccess
{
    public interface IStudentDataAccess
    {
        Task CreateNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();
        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(string id, UpdateStudent student);
        Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Student> patchDocument);

        Task<List<Student>> GetFilteredStudentsAsync(StudentFilterParameters studentFilterParameters);
        Task<long> GetFilteredStudentsCountAsync(StudentFilterParameters studentFilterParameters);
    }
}
