using A.Contracts.Entities;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using B1.RedisCache;
using C.BusinessLogic.ILoigcs;
using MassTransit;
using Microsoft.AspNetCore.JsonPatch;

namespace C.BusinessLogic.Logics
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IStudentDataAccess _studentsService;
        private readonly ICache _redisCache;
        private readonly ISharedDataAccess _sharedDataAccess;
        private readonly IAccountDataAccess _accountDataAccess;

        public StudentLogic(IStudentDataAccess studentsService, ICache redisCache, ISharedDataAccess sharedDataAccess)
        {
            _studentsService = studentsService;
            _redisCache = redisCache;
            _sharedDataAccess = sharedDataAccess;
        }

        private string GenerateCacheKey(StudentFilterParameters studentFilterParameters)
        {
            string cacheKey = "page-" + studentFilterParameters.PageNumber.ToString()+"size-"+ studentFilterParameters.PageSize.ToString();

            if(!string.IsNullOrEmpty(studentFilterParameters.Department)) { cacheKey += "dept-"+ studentFilterParameters.Department; }
            if (!string.IsNullOrEmpty(studentFilterParameters.Gender)) { cacheKey += "gender-" + studentFilterParameters.Gender; }
            if (!string.IsNullOrEmpty(studentFilterParameters.Session)) { cacheKey += "session-" + studentFilterParameters.Session; }
            if (!string.IsNullOrEmpty(studentFilterParameters.BloodGroup)) { cacheKey += "blood-" + studentFilterParameters.BloodGroup; }

            return cacheKey;
        }

        public async Task CreateNewStudentAsync(Student student)
        {
            await _studentsService.CreateNewStudentAsync(student);
            await _redisCache.ClearCache();
            return;
        }
        
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var cacheData = await _redisCache.GetData<List<Student>>("allStudents");
            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            var students = await _studentsService.GetAllStudentsAsync();

            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            _redisCache.SetData("allStudents", students, expiryTime);
            return students;
        }

        public  async Task<Student> GetStudent(string username)
        {
            return await _studentsService.GetStudentAsync(username);
        }

        public async Task<Tuple<List<Student>,long>> GetFilteredStudentsAsync(StudentFilterParameters studentFilterParameters)
        {
            try
            {
                var cacheKey = GenerateCacheKey(studentFilterParameters);
                var cacheData = await _redisCache.GetData<Tuple<List<Student>, long>>(cacheKey);

                if (cacheData != null)
                {
                    return cacheData;
                }

                List<Student> students = await _studentsService.GetFilteredStudentsAsync(studentFilterParameters);
                long count = await _studentsService.GetFilteredStudentsCountAsync(studentFilterParameters);

                _redisCache.SetData(cacheKey, Tuple.Create(students, count), DateTimeOffset.Now.AddSeconds(60));

                return Tuple.Create(students, count);
            }
            catch (Exception e)
            {
                List<Student> students = await _studentsService.GetFilteredStudentsAsync(studentFilterParameters);
                long count = await _studentsService.GetFilteredStudentsCountAsync(studentFilterParameters);

                return Tuple.Create(students, count);
            }
            
        }

        public async Task<bool> PartialUpdateAsync(string username, JsonPatchDocument<Student> patchDocument)
        {
            _redisCache.ClearCache();
            return await _studentsService.PartialUpdateAsync(username,  patchDocument); ;
        }

        public async Task<bool> UpdateStudentAsync(UpdateStudent student)
        {
            _redisCache.ClearCache();
            await _studentsService.UpdateStudentAsync(student);
            return true;
        }

        public async Task<bool> DeleteStudentAsync(string id)
        {
            var student = await _studentsService.GetStudentByIdAsync(id);
            return await _sharedDataAccess.DeleteUserAsync(student.Username);
        }
    }
}
