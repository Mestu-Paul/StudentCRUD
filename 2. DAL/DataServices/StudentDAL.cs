using _1.BOL.Entities;
using _2._DAL.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using _1.BOL.DBSettings;
using Microsoft.AspNetCore.JsonPatch;

namespace _2._DAL.DataServices
{
    public class StudentDAL : IStudentDAL
    {
        private readonly IMongoCollection<Students> _students;
        public StudentDAL(IOptions<DBSettings> MDBSettings)
        {
            MongoClient client = new MongoClient(MDBSettings.Value.ConnectionStrings);
            IMongoDatabase database = client.GetDatabase(MDBSettings.Value.DatabaseName);
            _students = database.GetCollection<Students>(MDBSettings.Value.CollectionName);
        }
        
        public async Task CreateAsync(Students students)
        {
            await _students.InsertOneAsync(students);
            return;
        }

        public async Task<List<Students>> GetAsync()
        {
            return await _students.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Students>.Filter.Eq("Id", id);
            var deleteResult = await _students.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(string id, Students student)
        {
            var filter = Builders<Students>.Filter.Eq("Id", id);
            var updateDefinition = Builders<Students>.Update
                .Set(s => s.Name, student.Name) 
                .Set(s => s.Department, student.Department)
                .Set(s => s.Session, student.Session)
                .Set(s => s.Gender, student.Gender);    

            var updateResult = await _students.UpdateOneAsync(filter, updateDefinition);

            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> PatchAsync(string id, JsonPatchDocument<Students> patchDocument)
        {
            var filter = Builders<Students>.Filter.Eq("Id", id);
            var student = await _students.Find(filter).FirstOrDefaultAsync();

            if (student == null)
            {
                return false; // Student not found
            }

            patchDocument.ApplyTo(student);

            var result = await _students.ReplaceOneAsync(filter, student);

            return result.ModifiedCount > 0;
        }

        public async Task<List<Students>> GetStudentsPageAsync(int pageNumber, int pageSize)
        {
            int skipCount = (pageNumber - 1) * pageSize;
            return await _students.Find(student => true)
                                           .Skip(skipCount)
                                           .Limit(pageSize)
                                           .ToListAsync();
        }
    }
}
