using Core.Attributes;
using Core.Interfaces;
using Core.Models;
using Core.Models.Account;
using Core.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(IAccountsService accountsService) : ControllerBase
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await accountsService.GetAll());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await accountsService.GetById(id));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getByUserName/{userName}")]
        public async Task<IActionResult> GetByUserName([FromRoute] string userName)
        {
            return Ok(await accountsService.GetByUserName(userName));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto register)
        {
            await accountsService.Register(register);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            return Ok(await accountsService.Login(login));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] LoginGoogleDto login)
        {
            return Ok(await accountsService.GoogleLogin(login));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await accountsService.Logout();
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AuthorizeUserPermissionAttribute]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            await accountsService.Delete(userId);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AuthorizeUserPermissionAttribute]
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update([FromRoute] string userId, [FromForm] UpdateUserDto updateUserDto)
        {
            await accountsService.Update(userId, updateUserDto);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            await accountsService.ChangePassword(changePassword);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromQuery] ItemsFilter itemsFilter)
        {
            return Ok(await accountsService.GetByFilter(itemsFilter));
        }
    }
}
