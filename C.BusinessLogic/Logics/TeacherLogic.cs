using A.Contracts.Entities;
using A.Contracts.Models;
using B.DatabaseAccess.DataAccess;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.Logics
{
    public class TeacherLogic : ITeacherLogic
    {
        private readonly ITeacherDataAccess _teacherDataAccess;
        private readonly ISharedDataAccess _sharedDataAccess;

        public TeacherLogic(ITeacherDataAccess teacherDataAccess, ISharedDataAccess sharedDataAccess)
        {
            _teacherDataAccess = teacherDataAccess;
            _sharedDataAccess = sharedDataAccess;
        }

        public async Task CreateNewTeacherAsync(Teacher teacher)
        {
            await _teacherDataAccess.CreateNewTeacherAsync(teacher);
            return;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await _teacherDataAccess.GetAllTeachersAsync();
        }

        public async Task<Tuple<List<Teacher>, long>> GetFilteredTeachersAsync(TeacherFilterParameters teacherFilterParameters)
        {
            List<Teacher> teachers = await _teacherDataAccess.GetFilteredTeachersAsync(teacherFilterParameters);
            long count = await _teacherDataAccess.GetFilteredTeachersCountAsync(teacherFilterParameters);

            return Tuple.Create(teachers, count);
        }

        public async Task<Teacher> GetTeacherAsync(string username)
        {
            return await _teacherDataAccess.GetTeacherAsync(username);
        }

        public async Task<bool> DeleteTeacherAsync(string id)
        {
            var student = await _teacherDataAccess.GetTeacherByIdAsync(id);
            return await _sharedDataAccess.DeleteUserAsync(student.Username);
        }

        public async Task<bool> UpdateTeacherAsync(Teacher teacher)
        {
            return await _teacherDataAccess.UpdateTeacherAsync(teacher);
        }

        public async Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Teacher> patchDocument)
        {
            return await _teacherDataAccess.PartialUpdateAsync(id, patchDocument);
        }
    }
}
