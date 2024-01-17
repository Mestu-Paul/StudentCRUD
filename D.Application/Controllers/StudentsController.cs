using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using D.Application.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace D.Application.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentLogic _studentLogic;

        public StudentController(IStudentLogic studentLogic)
        {
            _studentLogic = studentLogic;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewStudent([FromBody]Student student)
        {
            try
            {
                await _studentLogic.CreateNewStudentAsync(student);
                return StatusCode(201);
            }
            catch (FormatException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            StudentResponse _studentResponse = new StudentResponse();
            try
            {
                _studentResponse.students = await _studentLogic.GetAllStudentsAsync();
                _studentResponse.isSuccess = true;
                _studentResponse.message = "";
                return Ok(_studentResponse);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }

        [HttpGet("filterByPage")]
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
                return StatusCode(400, "Invalid page number and/or size");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }


        [HttpGet("customFilter")]
        public async Task<IActionResult> GetCustomFilteredStudents(int pageNumber, [FromQuery] string department,
            [FromQuery] string session, [FromQuery] string gender)
        {
            try
            {
                List<Student> students = await _studentLogic.GetCustomFilteredStudentsAsync(pageNumber, department, session, gender);
                return Ok(students);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
             
        }


        [HttpGet("countCustomFilter")]
        public async Task<IActionResult> GetCountCustomFilter([FromQuery] string department,
            [FromQuery] string session, [FromQuery] string gender)
        {
            try
            {
                return Ok(await _studentLogic.GetNumberOfCustomFilterStudentsAsync(department, session, gender));
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                return Ok(await _studentLogic.GetTotalNumberOfStudentsAsync());
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody]UpdateStudent student)
        {
            try
            {
                await _studentLogic.UpdateStudentAsync(id, student);
                return Ok("Updated students information");
            }
            catch (FormatException e)
            {
                return StatusCode(400,"Invalid input format");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }

        [HttpPatch("partialUpdate/{id}")]
        public async Task<IActionResult> PartialUpdateAsync(string id, [FromBody] JsonPatchDocument<Student> patchDocument)
        {
            if (string.IsNullOrEmpty(id) || patchDocument == null)
            {
                return BadRequest("Invalid ID or payload");
            }
            try
            {
                await _studentLogic.PartialUpdateAsync(id, patchDocument);
                return Ok("Updated students information");
            }
            catch (FormatException e)
            {
                return StatusCode(400, "Invalid input format");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(string id){
            try
            {
                bool isSuccess = await _studentLogic.DeleteStudentAsync(id);
                if (isSuccess)
                {
                    return Ok("Successfully deleted");
                }
                else
                {
                    return BadRequest("Invalid student's information format.");
                }
            }
            catch (FormatException e)
            {
                return StatusCode(400, "Invalid id format");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }
    }
}
