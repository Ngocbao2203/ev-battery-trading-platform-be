using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Listing;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;

        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] decimal from = 0, [FromQuery] decimal to = decimal.MaxValue, [FromQuery] EBTP.Repository.Enum.ListingStatusEnum? listingStatusEnum = null, [FromQuery] EBTP.Repository.Enum.CategoryEnum? categoryEnum = null)
        {
            var result = await _listingService.GetAllAsync(search, pageIndex, pageSize, from, to, listingStatusEnum, categoryEnum);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetByStatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] decimal from = 0, [FromQuery] decimal to = decimal.MaxValue, [FromQuery] EBTP.Repository.Enum.StatusEnum? status = null)
        {
            var result = await _listingService.GetListingsByStatusAsync(pageIndex, pageSize, from, to, status);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _listingService.GetByIdAsync(id);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateListing")]
        public async Task<IActionResult> CreateListing([FromForm] CreateListingDTO createListingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _listingService.CreateAsync(createListingDTO);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("VnpayUrl/{listingId}")]
        public async Task<IActionResult> CreateVnPayUrl(Guid listingId)
        {
            var result = await _listingService.CreateVnPayUrlAsync(listingId, HttpContext);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var result = await _listingService.HandleVnPayReturnAsync(Request.Query);
            return Ok(result);
        }
    }
}
