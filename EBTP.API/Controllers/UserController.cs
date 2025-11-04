using EBTP.Service.DTOs.User;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var getUser = await _userService.GetCurrentUserById();
            return Ok(getUser);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetCurrentUserByUserId([FromQuery] Guid userId)
        {
            var getUser = await _userService.GetUserByUserId(userId);
            return Ok(getUser);
        }

        [HttpPut("UpdateInfoUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateInfoUserDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _userService.UpdateUserInfoAsync(updateDto);

            if (result.Error != 0)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
