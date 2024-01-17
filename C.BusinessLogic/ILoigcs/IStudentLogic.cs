using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;

namespace C.BusinessLogic.ILoigcs
{
    public interface IStudentLogic
    {
        Task CreateNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();
        Task<List<Student>> GetStudentsPagedAsync(int pageNumber, int pageSize);
        Task<List<Student>> GetCustomFilteredStudentsAsync(int pageNumber, string filterBy, string filterText);
        Task<List<Student>> GetCustomFilteredStudentsAsync(int pageNumber, string department, string session, string gender);
        Task<long> GetNumberOfCustomFilterStudentsAsync(string department, string session, string gender);
        Task<long> GetTotalNumberOfStudentsAsync();
        Task<bool> DeleteStudentAsync(string id);
        Task<bool> UpdateStudentAsync(string id, UpdateStudent student);
        Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Student> patchDocument);
    }
}
