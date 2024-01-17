using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using B1.RedisCache;
using C.BusinessLogic.ILoigcs;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using StackExchange.Redis;

namespace C.BusinessLogic.Logics
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IStudentDataAccess _studentsService;
        private readonly ICache _redisCache;

        public StudentLogic(IStudentDataAccess studentsService, ICache redisCache)
        {
            _studentsService = studentsService;
            _redisCache = redisCache;
        }

        public async Task CreateNewStudentAsync(Student student)
        {
            await _studentsService.CreateNewStudentAsync(student);
            _redisCache.ClearCache();
            return;
        }
        
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Student>> GetStudentsPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid page number or size");
            }
            try
            {
                var cacheData = await _redisCache.GetData<List<Student>>(pageNumber,pageSize);
                if (cacheData != null && cacheData.Count() > 0)
                {
                    return cacheData;
                }

                var students = await _studentsService.GetStudentsPagedAsync(pageNumber, pageSize);

                var expiryTime = DateTimeOffset.Now.AddMinutes(30);
                _redisCache.SetData(pageNumber, pageSize, students, expiryTime);
                return students;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<List<Student>> GetCustomFilteredStudentsAsync(int pageNumber, string filterBy, string filterText)
        {
            return await _studentsService.GetCustomFilteredStudentsAsync(pageNumber, filterBy, filterText);
        }
        public async Task<List<Student>> GetCustomFilteredStudentsAsync(int pageNumber, string department, string session, string gender)
        {
            return await _studentsService.GetCustomFilteredStudentsAsync(pageNumber, department, session, gender);
        }

        public async Task<long> GetNumberOfCustomFilterStudentsAsync(string department, string session, string gender)
        {
            return await _studentsService.GetNumberOfCustomFilterStudentsAsync(department, session, gender);
        }

        public async Task<long> GetTotalNumberOfStudentsAsync()
        {
            try
            {
                 var cacheData = await _redisCache.GetData<long>("numberOfStudents");
                if (cacheData != null)
                {
                    return cacheData;
                }
                cacheData = await _studentsService.GetTotalNumberOfStudentsAsync();
                await _redisCache.SetData("numberOfStudents", cacheData, DateTimeOffset.Now.AddMinutes(5));
                return cacheData;
            }
            catch (RedisConnectionException e)
            {
                return await _studentsService.GetTotalNumberOfStudentsAsync();
            }
            
        }

        public async Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Student> patchDocument)
        {
            _redisCache.ClearCache();
            return await _studentsService.PartialUpdateAsync(id,  patchDocument); ;
        }

        public async Task<bool> UpdateStudentAsync(string id, UpdateStudent student)
        {
            _redisCache.ClearCache();
            return await _studentsService.UpdateStudentAsync(id, student); ;
        }

        public async Task<bool> DeleteStudentAsync(string id)
        {
            _redisCache.ClearCache();
            return await _studentsService.DeleteStudentAsync(id); ;
        }
    }
}
