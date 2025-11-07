using EBTP.Repository.Enum;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IUserService _userService;

        public StaffController(IListingService listingService, IUserService userService)
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
        public async Task<IActionResult> RejectListing(Guid listingId, [FromQuery] ResonRejectListingEnum? resonReject, [FromQuery] string? reason)
        {
            var result = await _listingService.RejectListingAsync(listingId, (ResonRejectListingEnum)resonReject, reason);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Ban-User/{userId}")]
        public async Task<IActionResult> BanUser(Guid userId, string banDescription)
        {
            var result = await _userService.BanUser(userId, banDescription);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("UbBan-User/{userId}")]
        public async Task<IActionResult> UnBanUser(Guid userId)
        {
            var result = await _userService.UnBanUser(userId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
