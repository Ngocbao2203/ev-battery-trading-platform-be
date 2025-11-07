using EBTP.Repository.Enum;
using EBTP.Service.IServices;
using EBTP.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IUserService _userService;

        public AdminController(IListingService listingService, IUserService userService)
        {
            _listingService = listingService;
            _userService = userService;
        }

        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetAllUsers(pageIndex, pageSize);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
