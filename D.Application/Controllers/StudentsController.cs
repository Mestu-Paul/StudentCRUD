using A.Contracts.Contracts;
using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Controller]
    [Route("api/[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentLogic _studentLogic;

        public StudentsController(IStudentLogic studentLogic)
        {
            _studentLogic = studentLogic;
        }

        [HttpPost]
        public async Task<StudentResponse> CreateNewStudent([FromBody]Student student)
        {
            return await _studentLogic.CreateNewStudentAsync(student);
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
