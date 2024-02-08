using A.Contracts.Entities;
using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;

namespace C.BusinessLogic.ILoigcs
{
    public interface IStudentLogic
    {
        Task CreateNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();

        Task<Student> GetStudent(string username);

        Task<Tuple<List<Student>, long>> GetFilteredStudentsAsync(StudentFilterParameters studentFilterParameters);

        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(UpdateStudent student);
        Task<bool> PartialUpdateAsync(string username, JsonPatchDocument<Student> patchDocument);
    }
}
