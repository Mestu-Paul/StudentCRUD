using _1.BOL.Entities;
using _3._BLL.ILogics;
using _3._BLL.Logics;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace _4._Application.Controllers
{
    [Controller]
    [Route("api/[controller]/[action]")]
    public class StudentsController : Controller
    {
        private readonly IStudentLogic _studentLogic;

        public StudentsController(IStudentLogic studentLogic)
        {
            _studentLogic = studentLogic;
        }

        [HttpGet]
        public async Task<List<Students>> GetStudent()
        {
            return await _studentLogic.GetAllStudentsAsync();

        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsPage(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var paginatedStudents = await _studentLogic.GetStudentsPageAsync(pageNumber, pageSize);
                return Ok(paginatedStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Students students) {
            try
            {
                await _studentLogic.CreateStudentAsync(students);
                return CreatedAtAction(nameof(GetStudent), new { id = students.Id }, students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            // await _studentLogic.DeleteStudentAsync(id);
            // return Ok("Successfully deleted");
            
            bool deletionResult = await _studentLogic.DeleteStudentAsync(id);
            if (deletionResult){
                return Ok("Student deleted successfully");
            }
            else{
                return NotFound("Student not found or deletion failed");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody]Students student)
        {
            bool updateResult = await _studentLogic.UpdateStudentAsync(id, student);

            if (updateResult)
            {
                return Ok($"Student updated successfully {id}");
            }
            else
            {
                return NotFound($"Student not found or update failed {id}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStudent(string id, [FromBody] JsonPatchDocument<Students> patchDocument)
        {
            if (string.IsNullOrEmpty(id) || patchDocument == null)
            {
                return BadRequest("Invalid ID or payload");
            }

            var updateResult = await _studentLogic.PatchStudentAsync(id, patchDocument);

            if (updateResult)
            {
                return Ok("Student patched successfully");
            }
            else
            {
                return NotFound("Student not found or patch failed");
            }
        }
    }
}
