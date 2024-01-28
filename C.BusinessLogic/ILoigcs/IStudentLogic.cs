using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;

namespace C.BusinessLogic.ILoigcs
{
    public interface IStudentLogic
    {
        Task CreateNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();

        Task<Tuple<List<Student>, long>> GetFilteredStudentsAsync(StudentFilterParameters studentFilterParameters);

        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(string id, UpdateStudent student);
        Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Student> patchDocument);
    }
}
