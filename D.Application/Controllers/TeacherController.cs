using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using C.BusinessLogic.Logics;
using D.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Controller]
    [Route("api/[controller]")]


    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherLogic _teacherLogic;
        public TeacherController(ITeacherLogic teacherLogic)
        {
            _teacherLogic = teacherLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _teacherLogic.GetAllTeachersAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredTeacher([FromQuery] TeacherFilterParameters teacherFilterParameters)
        {
            try
            {
                FilterResponse<Teacher> filterResponse = new FilterResponse<Teacher>(await _teacherLogic.GetFilteredTeachersAsync(teacherFilterParameters),
                    teacherFilterParameters.PageSize);
                return Ok(filterResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Teacher teacher)
        {
            try
            {
                await _teacherLogic.CreateNewTeacherAsync(teacher);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] Teacher teacher)
        {
            try
            {
                bool isSuccess = await _teacherLogic.UpdateTeacherAsync(id, teacher);
                if(isSuccess) 
                    return Ok();
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("partialUpdate/{id}")]
        public async Task<IActionResult> PartialUpdateAsync(string id, [FromBody] JsonPatchDocument<Teacher> patchDocument)
        {
            if (string.IsNullOrEmpty(id) || patchDocument == null)
            {
                return BadRequest("Invalid ID or payload");
            }
            try
            {
                bool isSuccess = await _teacherLogic.PartialUpdateAsync(id, patchDocument);
                if (isSuccess)
                    return Ok();
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                bool isSuccess = await _teacherLogic.DeleteTeacherAsync(id);
                if (isSuccess)
                    return Ok();

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
