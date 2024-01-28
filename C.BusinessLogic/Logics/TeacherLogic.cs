using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.Logics
{
    public class TeacherLogic : ITeacherLogic
    {
        private readonly ITeacherDataAccess _teacherDataAccess;
        public TeacherLogic(ITeacherDataAccess teacherDataAccess)
        {
            _teacherDataAccess = teacherDataAccess;
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

        public async Task<bool> DeleteTeacherAsync(string id)
        {
            return await _teacherDataAccess.DeleteTeacherAsync(id);
        }

        public async Task<bool> UpdateTeacherAsync(string id, Teacher teacher)
        {
            return await _teacherDataAccess.UpdateTeacherAsync(id, teacher);
        }

        public async Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Teacher> patchDocument)
        {
            return await _teacherDataAccess.PartialUpdateAsync(id, patchDocument);
        }
    }
}
