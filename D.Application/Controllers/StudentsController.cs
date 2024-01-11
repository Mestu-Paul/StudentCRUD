using A.Contracts.Contracts;
using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using D.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using StudentResponse = A.Contracts.Contracts.StudentResponse;

namespace D.Application.Controllers
{
    [Controller]
    [Route("api/[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentLogic _studentLogic;
        private readonly StudentResponse _studentResponse = new StudentResponse();

        public StudentsController(IStudentLogic studentLogic)
        {
            _studentLogic = studentLogic;
        }

        [HttpPost]
        public async Task<StudentResponse> CreateNewStudent([FromBody]Student student)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;

            student.CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"); ;
            student.LastUpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                await _studentLogic.CreateNewStudentAsync(student);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Created a new student";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while creating a new student";
            }
            return _studentResponse;
        }

        [HttpGet]
        public async Task<StudentResponse> GetAllStudents()
        {
            return await _studentLogic.GetAllStudentsAsync();
        }

        [HttpGet]
        public async Task<StudentResponse> GetStudents(int pageNumber, int pageSize)
        {
            return await _studentLogic.GetStudentsPagedAsync(pageNumber,pageSize);
        }

        [HttpGet]
        public async Task<long> GetTotalNumberOfStudents()
        {
            return await _studentLogic.GetTotalNumberOfStudentsAsync();
        }

        [HttpPut("{id}")]
        public async Task<StudentResponse> UpdateStudent(string id, [FromBody]Student student)
        {
            return await _studentLogic.UpdateStudentAsync(id, student);
        }

        [HttpPatch("{id}")]
        public async Task<StudentResponse> UpdateStudent(string id, string fieldName, string fieldValue)
        {
            return await _studentLogic.UpdateStudentSingleAttributeAsync(id,fieldName,fieldValue);
        }

        [HttpDelete("{id}")]
        public async Task<StudentResponse> DeleteStudent(string id)
        {
            return await _studentLogic.DeleteStudentAsync(id);
        }
    }
}
