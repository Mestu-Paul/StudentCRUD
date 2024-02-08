using A.Contracts.DBSettings;
using A.Contracts.Entities;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace B.DatabaseAccess.DataAccess
{
    public class TeacherDataAccess : ITeacherDataAccess
    {
        private readonly IMongoCollection<Teacher> _teachersCollection;
        public TeacherDataAccess(IOptions<MongoDBSetting> mongoDBSetting)
        {
            string collectionName = "teachers";
            MongoClient client = new MongoClient(mongoDBSetting.Value.ConnectionStrings);
            IMongoDatabase database = client.GetDatabase(mongoDBSetting.Value.DatabaseName);
            _teachersCollection = database.GetCollection<Teacher>(collectionName);
        }

        private FilterDefinition<Teacher> GetCombinedFilter(TeacherFilterParameters teacherFilterParameters)
        {
            var filterBuilder = Builders<Teacher>.Filter;
            var combinedFilter = filterBuilder.Empty;
            if (!string.IsNullOrEmpty(teacherFilterParameters.Department))
            {
                var departmentFilter = filterBuilder.Eq("Department", teacherFilterParameters.Department);
                combinedFilter = filterBuilder.And(combinedFilter, departmentFilter);
            }

            if (!string.IsNullOrEmpty(teacherFilterParameters.Gender))
            {
                var genderFilter = filterBuilder.Eq("Gender", teacherFilterParameters.Gender);
                combinedFilter = filterBuilder.And(combinedFilter, genderFilter);
            }

            if (!string.IsNullOrEmpty(teacherFilterParameters.Education))
            {
                var textToSearch = "/.*" + teacherFilterParameters.Education + ".*/i";
                var genderFilter = filterBuilder.Regex(x => x.Education, new BsonRegularExpression(textToSearch));
                combinedFilter = filterBuilder.And(combinedFilter, genderFilter);
            }

            if (!string.IsNullOrEmpty(teacherFilterParameters.Research))
            {
                var textToSearch = "/.*" + teacherFilterParameters.Research + ".*/i";
                var genderFilter = filterBuilder.Regex(x => x.Research, new BsonRegularExpression(textToSearch));
                combinedFilter = filterBuilder.And(combinedFilter, genderFilter);
            }

            return combinedFilter;
        }

        public async Task CreateNewTeacherAsync(Teacher teacher)
        {
            await _teachersCollection.InsertOneAsync(teacher);
            return;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await _teachersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Teacher> GetTeacherAsync(string username)
        {
            return await _teachersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task<Teacher> GetTeacherByIdAsync(string id)
        {
            return await _teachersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Teacher>> GetFilteredTeachersAsync(TeacherFilterParameters teacherFilterParameters)
        {
            var combinedFilter = GetCombinedFilter(teacherFilterParameters);

            int skipCount = (teacherFilterParameters.PageNumber - 1) * teacherFilterParameters.PageSize;

            var filteredStudents = await _teachersCollection
                .Find(combinedFilter)
                .Skip(skipCount)
                .Limit(teacherFilterParameters.PageSize)
                .ToListAsync();

            return filteredStudents;
        }

        public async Task<long> GetFilteredTeachersCountAsync(TeacherFilterParameters teacherFilterParameters)
        {
            var combinedFilter = GetCombinedFilter(teacherFilterParameters);
            return await _teachersCollection.Find(combinedFilter).CountDocumentsAsync();
        }

        public async Task<bool> DeleteTeacherAsync(string id)
        {
            var result = await _teachersCollection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }


        public async Task<bool> UpdateTeacherAsync(string id, Teacher teacher)
        {
            var filter = Builders<Teacher>.Filter.Eq("Id", id);
            var updateDefinition = Builders<Teacher>.Update
                    .Set(s => s.Name, teacher.Name)
                    .Set(s => s.Department, teacher.Department)
                    .Set(s => s.Phone, teacher.Phone)
                    .Set(s => s.Education, teacher.Education)
                    .Set(s => s.Research, teacher.Research)
                ;

            var updateResult = await _teachersCollection.UpdateOneAsync(filter, updateDefinition);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Teacher> patchDocument)
        {
            var filter = Builders<Teacher>.Filter.Eq("Id", id);
            var student = await _teachersCollection.Find(filter).FirstOrDefaultAsync();

            if (student == null)
            {
                return false;
            }

            patchDocument.ApplyTo(student);

            var result = await _teachersCollection.ReplaceOneAsync(filter, student);
            return result.ModifiedCount > 0;
        }
    }
}
