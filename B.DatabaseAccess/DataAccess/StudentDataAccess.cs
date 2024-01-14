using A.Contracts.DBSettings;
using A.Contracts.Models;
using B.DatabaseAccess.IDataAccess;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Xml.XPath;

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


        public async Task<List<Student>> GetStudentsPagedAsync(int pageNumber, int pageSize)
        {
            int skipCount = (pageNumber - 1) * pageSize;
            return await _studentsCollection.Find(student => true)
                .Skip(skipCount)
                .Limit(pageSize)
                .ToListAsync();
        }


        public async Task<long> GetTotalNumberOfStudentsAsync()
        {
            return await _studentsCollection.EstimatedDocumentCountAsync();
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

        public async Task<bool> UpdateStudentSingleAttributeAsync(string id, JsonPatchDocument<Student> patchDocument)
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

        // delete a Single student Information from students collection using unique id
        // if no data deleted return 0 otherwise 1
        public async Task<bool> DeleteStudentAsync(string id)
        {
            var filter = Builders<Student>.Filter.Eq("Id", id);
            var deleteResult = await _studentsCollection.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;
        }
    }
}
