using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using D.Application.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
            try
            {
                _studentResponse.students = await _studentLogic.GetAllStudentsAsync();
                _studentResponse.isSuccess = true;
                _studentResponse.message = "";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something error while call GetAllStudents API";
                _studentResponse.isSuccess = false;
                _studentResponse.students = null;
            }
            return _studentResponse;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(int pageNumber, int pageSize)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            
            try
            {
                _studentResponse.students = await _studentLogic.GetStudentsPagedAsync(pageNumber, pageSize);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "";
                return Ok(_studentResponse);
            }
            catch (ArgumentOutOfRangeException e)
            {
                _studentResponse.message = e.Message;
                
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while call GetStudents API";
            }
            return BadRequest(_studentResponse);
            
        }

        [HttpGet]
        public async Task<long> GetTotalNumberOfStudents()
        {
            return await _studentLogic.GetTotalNumberOfStudentsAsync();
        }

        [HttpPut("{id}")]
        public async Task<StudentResponse> UpdateStudent(string id, [FromBody]Student student)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            try
            {
                await _studentLogic.UpdateStudentAsync(id, student);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Updated students information";
            }
            catch (FormatException e)
            {
                _studentResponse.message = e.Message;
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while updating students information";
            }
            return _studentResponse;
        }

        [HttpPatch("{id}")]
        public async Task<StudentResponse> UpdateStudent(string id, JsonPatchDocument<Student> patchDocument)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            try
            {
                await _studentLogic.UpdateStudentSingleAttributeAsync(id, patchDocument);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Updated students information";
            }
            catch (FormatException e)
            {
                _studentResponse.message = e.Message;
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while updating students information";
            }

            return _studentResponse;
        }

        [HttpDelete("{id}")]
        public async Task<StudentResponse> DeleteStudent(string id)
        {
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            try
            {
                bool isDeleted = await _studentLogic.DeleteStudentAsync(id);
                _studentResponse.isSuccess |= isDeleted;
                if (isDeleted)
                {
                    _studentResponse.message = "Successfully deleted";
                }
                else
                {
                    _studentResponse.message = "Invalid student information";
                }
            }
            catch (FormatException e)
            {
                _studentResponse.message = "Invalid  id format";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while deleting a students information";
            }

            return _studentResponse;
        }
    }
}
