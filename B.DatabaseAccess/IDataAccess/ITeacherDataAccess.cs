using A.Contracts.Entities;
using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace B.DatabaseAccess.IDataAccess
{
    public interface ITeacherDataAccess
    {
        Task CreateNewTeacherAsync(Teacher teacher);

        Task<List<Teacher>> GetAllTeachersAsync();
        Task<long> GetTeachersCount();

        Task<Teacher> GetTeacherAsync(string username);

        Task<Teacher> GetTeacherByIdAsync(string id);

        Task<bool> UpdateTeacherAsync(Teacher teacher);

        Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Teacher> patchDocument);


        Task<List<Teacher>> GetFilteredTeachersAsync(TeacherFilterParameters teacherFilterParameters);

        Task<long> GetFilteredTeachersCountAsync(TeacherFilterParameters teacherFilterParameters);

        Task<bool> DeleteTeacherAsync(string id);
    }
}
