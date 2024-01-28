using A.Contracts.DBSettings;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace B.DatabaseAccess.DataAccess
{
    public class StudentDataAccess : IStudentDataAccess
    {
        private readonly IMongoCollection<Student> _studentsCollection;

        public StudentDataAccess(IOptions<MongoDBSetting> mongoDbSetting)
        {
            string studentCollection = "students";
            MongoClient client = new MongoClient(mongoDbSetting.Value.ConnectionStrings);
            IMongoDatabase database = client.GetDatabase(mongoDbSetting.Value.DatabaseName);
            _studentsCollection = database.GetCollection<Student>(studentCollection);
        }

        private FilterDefinition<Student> GetCombinedFilter(StudentFilterParameters studentFilterParameters)
        {
            var filterBuilder = Builders<Student>.Filter;
            var combinedFilter = filterBuilder.Empty;
            if (!string.IsNullOrEmpty(studentFilterParameters.Department))
            {
                var departmentFilter = filterBuilder.Eq("Department", studentFilterParameters.Department);
                combinedFilter = filterBuilder.And(combinedFilter, departmentFilter);
            }

            if (!string.IsNullOrEmpty(studentFilterParameters.Session))
            {
                var sessionFilter = filterBuilder.Eq("Session", studentFilterParameters.Session);
                combinedFilter = filterBuilder.And(combinedFilter, sessionFilter);
            }

            if (!string.IsNullOrEmpty(studentFilterParameters.Gender))
            {
                var genderFilter = filterBuilder.Eq("Gender", studentFilterParameters.Gender);
                combinedFilter = filterBuilder.And(combinedFilter, genderFilter);
            }

            if (!string.IsNullOrEmpty(studentFilterParameters.BloodGroup))
            {
                var genderFilter = filterBuilder.Eq("BloodGroup", studentFilterParameters.BloodGroup);
                combinedFilter = filterBuilder.And(combinedFilter, genderFilter);
            }

            return combinedFilter;
        }

        public async Task CreateNewStudentAsync(Student student)
        {
            student.CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"); ;
            student.LastUpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            await _studentsCollection.InsertOneAsync(student);
            return;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<Student>> GetFilteredStudentsAsync(StudentFilterParameters studentFilterParameters)
        {

            var combinedFilter = GetCombinedFilter(studentFilterParameters);

            int skipCount = (studentFilterParameters.PageNumber - 1) * studentFilterParameters.PageSize;
            
            var filteredStudents = await _studentsCollection
                .Find(combinedFilter)
                .Skip(skipCount)
                .Limit(studentFilterParameters.PageSize)
                .ToListAsync();

            return filteredStudents;
        }
        
        public async Task<long> GetFilteredStudentsCountAsync(StudentFilterParameters studentFilterParameters)
        {
            var combinedFilter = GetCombinedFilter(studentFilterParameters);
            return await _studentsCollection.Find(combinedFilter).CountDocumentsAsync();
        }

        public async Task<bool> UpdateStudentAsync(string id, UpdateStudent student)
        {
            var filter = Builders<Student>.Filter.Eq("Id", id);
            var updateDefinition = Builders<Student>.Update
                .Set(s => s.Name, student.Name)
                .Set(s => s.Department, student.Department)
                .Set(s => s.Session, student.Session)
                .Set(s => s.Phone, student.Phone)
                .Set(s => s.LastDonatedAt, student.LastDonatedAt)
                .Set(s => s.Address, student.Address)
                .Set(s => s.LastUpdatedAt, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
                ;

            var updateResult = await _studentsCollection.UpdateOneAsync(filter, updateDefinition);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> PartialUpdateAsync(string id, JsonPatchDocument<Student> patchDocument)
        {
            var filter = Builders<Student>.Filter.Eq("Id", id);
            var student = await _studentsCollection.Find(filter).FirstOrDefaultAsync();
            student.LastUpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            if (student == null)
            {
                return false; 
            }

            Console.WriteLine("Before Patch: " + JsonConvert.SerializeObject(student));

            patchDocument.ApplyTo(student);

            Console.WriteLine("After Patch: " + JsonConvert.SerializeObject(student));

            var result = await _studentsCollection.ReplaceOneAsync(filter, student);

            Console.WriteLine(result);
            return result.ModifiedCount > 0;
        }


        public async Task<bool> DeleteStudentAsync(string id)
        {
            var filter = Builders<Student>.Filter.Eq("Id", id);
            var deleteResult = await _studentsCollection.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;
        }
    }
}
