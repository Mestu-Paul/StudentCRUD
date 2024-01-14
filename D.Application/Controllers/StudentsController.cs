using A.Contracts.Models;
using B1.RedisCache;
using C.BusinessLogic.ILoigcs;
using D.Application.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
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
        public async Task<IActionResult> CreateNewStudent([FromBody]Student student)
        {
            StudentResponse _studentResponse = new StudentResponse();
            _studentResponse.isSuccess = false;
            _studentResponse.students = null;
            try
            {
                await _studentLogic.CreateNewStudentAsync(student);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Created a new student";
                return StatusCode(201, _studentResponse);
            }
            catch (FormatException e)
            {
                _studentResponse.message = e.Message;
                return StatusCode(400, _studentResponse);
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while creating a new student";
                return StatusCode(500, _studentResponse);
            }
        }

        [HttpGet]
        public async Task<StudentResponse> GetAllStudents()
        {
            StudentResponse _studentResponse = new StudentResponse();
            try
            {
                _studentResponse.students = await _studentLogic.GetAllStudentsAsync();
                _studentResponse.isSuccess = true;
                _studentResponse.message = "";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something error while call GetAllStudents API";
            }
            return _studentResponse;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(int pageNumber, int pageSize)
        {
            StudentResponse _studentResponse = new StudentResponse();
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
        public async Task<IActionResult> GetTotalNumberOfStudents()
        {
            try
            {
                return Ok(await _studentLogic.GetTotalNumberOfStudentsAsync());
            }
            catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<StudentResponse> UpdateStudent(string id, [FromBody]Student student)
        {
            StudentResponse _studentResponse = new StudentResponse();
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
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] JsonPatchDocument<Student> patchDocument)
        {
            if (string.IsNullOrEmpty(id) || patchDocument == null)
            {
                return BadRequest("Invalid ID or payload");
            }
            StudentResponse _studentResponse = new StudentResponse();
            try
            {
                await _studentLogic.UpdateStudentSingleAttributeAsync(id, patchDocument);
                _studentResponse.isSuccess = true;
                _studentResponse.message = "Updated students information";
                return Ok(_studentResponse);
            }
            catch (FormatException e)
            {
                _studentResponse.message = e.Message;
            }
            catch (JsonPatchException e)
            {
                _studentResponse.message = e.Message;
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while updating students information";
            }
            return BadRequest(_studentResponse);

        }

        [HttpDelete("{id}")]
        public async Task<StudentResponse> DeleteStudent(string id)
        {
            StudentResponse _studentResponse = new StudentResponse();
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
                    _studentResponse.message = "Invalid student's information format.";
                }
            }
            catch (FormatException e)
            {
                _studentResponse.message = "Invalid  id format";
            }
            catch (Exception e)
            {
                _studentResponse.message = "Something wrong while deleting a student's information";
            }

            return _studentResponse;
        }
    }
}
