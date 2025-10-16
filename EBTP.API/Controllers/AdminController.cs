using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IListingService _listingService;

        public AdminController(IListingService listingService)
        {
            _listingService = listingService;
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
        public async Task<IActionResult> RejectListing(Guid listingId, [FromQuery] string? reason)
        {
            var result = await _listingService.RejectListingAsync(listingId, reason);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
