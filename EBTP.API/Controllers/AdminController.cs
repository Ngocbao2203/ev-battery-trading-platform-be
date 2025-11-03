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

        [HttpPost("Accept-Listing/{listingId}")]
        public async Task<IActionResult> AcceptListing(Guid listingId)
        {
            var result = await _listingService.AcceptListingAsync(listingId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Reject-Listing/{listingId}")]
        public async Task<IActionResult> RejectListing(Guid listingId, [FromQuery] ResonRejectListingEnum reason, [FromQuery] string descriptionReject)
        {
            var result = await _listingService.RejectListingAsync(listingId, reason, descriptionReject);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        returnn Ok(result);
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
