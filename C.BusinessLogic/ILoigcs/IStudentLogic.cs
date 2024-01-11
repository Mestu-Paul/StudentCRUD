using A.Contracts.Contracts;
using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.ILoigcs
{
    public interface IStudentLogic
    {
        Task CreateNewStudentAsync(Student student);
        Task<StudentResponse> GetAllStudentsAsync();
        Task<StudentResponse> DeleteStudentAsync(string id);
        Task<StudentResponse> UpdateStudentAsync(string id, Student student);
        Task<StudentResponse> UpdateStudentSingleAttributeAsync(string id, string fieldName, string fieldValue);
        Task<StudentResponse> GetStudentsPagedAsync(int pageNumber, int pageSize);
        Task<long> GetTotalNumberOfStudentsAsync();
    }
}
