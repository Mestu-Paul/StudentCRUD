using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using D.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace D.Application.Controllers
{
    [Controller]
    [Route("api/[controller]")]


    [Authorize]
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
                return BadRequest("bad request"+e.Message);
            }
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                List<Student> students = await _studentLogic.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Something wrong : {e.Message}");
            }
        }



        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredStudents([FromQuery]StudentFilterParameters studentFilterParameters)
        {
            try
            {
                FilterResponse<Student> filterResponse = new FilterResponse<Student>(await _studentLogic.GetFilteredStudentsAsync(studentFilterParameters), studentFilterParameters.PageSize);
                return Ok(filterResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
                return Ok();
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
                    return Ok();
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
