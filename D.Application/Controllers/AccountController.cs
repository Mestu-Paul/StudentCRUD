using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using C.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountLogic _accountLogic;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountLogic accountLogic, ITokenService tokenService)
        {
            _accountLogic = accountLogic;
            _tokenService = tokenService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _accountLogic.GetUsersAsync());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForm userForm)
        {
            try
            {
                await _accountLogic.CreateUser(userForm.Username, userForm.Password, userForm.Role);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForm userForm)
        {
            try
            {
                return Ok(await _accountLogic.Login(userForm.Username, userForm.Password));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPut("updateRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserForm userForm)
        {
            try
            {
                await _accountLogic.UpdateUserRole(userForm.Username, userForm.Role);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                await _accountLogic.DeleteUser(username);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
