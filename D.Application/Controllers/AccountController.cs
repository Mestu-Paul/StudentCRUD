using A.Contracts.DataTransferObjects;
using A.Contracts.Models;
using C.BusinessLogic.ILoigcs;
using D.Application.Services;
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

        [Authorize]
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
                User user = await _accountLogic.Login(userForm.Username, userForm.Password);
                if (user != null)
                {
                    UserDTO userDTO = new UserDTO
                    {
                        Username = userForm.Username,
                        Token = _tokenService.CreateToken(user)
                    };
                    return Ok(userDTO);
                }
                return BadRequest("Invalid user");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpPut("updateRole")]
        public async Task<IActionResult> UpdateUserRole(string username, string newRole)
        {
            try
            {
                await _accountLogic.UpdateUserRole(username, newRole);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpDelete("delete")]
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
