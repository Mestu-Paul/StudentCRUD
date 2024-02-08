using A.Contracts.Entities;
using A.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.ILoigcs
{
    public interface ITeacherLogic
    {
        Task CreateNewTeacherAsync(Teacher teacher);
        Task<List<Teacher>> GetAllTeachersAsync();

        Task<Tuple<List<Teacher>, long>> GetFilteredTeachersAsync(TeacherFilterParameters teacherFilterParameters);
        Task<Teacher> GetTeacherAsync(string username);

        Task<bool> DeleteTeacherAsync(string id);
        Task<bool> UpdateTeacherAsync(string id, Teacher teacher);
        Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Teacher> patchDocument);
    }
}
