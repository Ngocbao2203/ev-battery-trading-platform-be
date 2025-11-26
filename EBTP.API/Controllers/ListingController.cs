using EBTP.Repository.Enum;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Listing;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IUnitOfWork _unitOfWork;

        public ListingController(IListingService listingService, IUnitOfWork unitOfWork)
        {
            _listingService = listingService;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] decimal? from = null, [FromQuery] decimal? to = null, [FromQuery] EBTP.Repository.Enum.ListingStatusEnum? listingStatusEnum = null, [FromQuery] EBTP.Repository.Enum.CategoryEnum? categoryEnum = null)
        {
            var result = await _listingService.GetAllAsync(search, pageIndex, pageSize, from, to, listingStatusEnum, categoryEnum);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetByStatus")]
        public async Task<IActionResult> GetByStatus([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] decimal? from = null, [FromQuery] decimal? to = null, [FromQuery] EBTP.Repository.Enum.StatusEnum? status = null)
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
        [HttpPut("UpdateListing")]
        public async Task<IActionResult> UpdateListing([FromForm] UpdateListingDTO updateListingDTO)
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
            var result = await _listingService.UpdateAsync(updateListingDTO);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // New: Delete listing by id
        [HttpDelete("Delete/{listingId}")]
        public async Task<IActionResult> DeleteListing([FromRoute] Guid listingId)
        {
            var result = await _listingService.DeleteAsync(listingId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ComfirmedSold/{listingId}")]
        public async Task<IActionResult> ComfirmedSold([FromRoute] Guid listingId)
        {
            var result = await _listingService.ConfirmedSold(listingId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("MyListings")]
        [Authorize] // Đảm bảo user đã login
        public async Task<ActionResult<Result<List<ListingDTO>>>> GetMyListings([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _listingService.GetMyListingsAsync(pageIndex, pageSize);
            return result.Error == 0 ? Ok(result) : Unauthorized(result);
        }
        [HttpGet("VnpayUrl/{listingId}")]
        public async Task<IActionResult> CreateVnPayUrl(Guid listingId)
        {
            var result = await _listingService.CreateVnPayUrlAsync(listingId, HttpContext);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }
        [HttpGet("Repayment/{listingId}")]
        public async Task<IActionResult> Repayment(Guid listingId)
        {
            var result = await _listingService.RetryPayment(listingId, HttpContext);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var query = HttpContext.Request.Query;

            // lấy payment type để biết gọi hàm nào
            var transactionNo = query["vnp_TxnRef"].ToString();
            var payment = await _unitOfWork.paymentRepository.GetByTransactionNoAsync(transactionNo);

            if (payment.PaymentTypeEnum == PaymentTypeEnum.ListingBuy)
            {
                var result = await _listingService.HandlePostListingPaymentAsync(query);
                var status = result.Error == 0 ? "success" : "failed";
                return Redirect($"https://voltx-three.vercel.app/payment-{status}");
            }

            if (payment.PaymentTypeEnum == PaymentTypeEnum.ListingPost)
            {
                var result = await _listingService.HandleVnPayReturnAsync(query);
                var status = result.Error == 0 ? "success" : "failed";
                return Redirect($"https://voltx-three.vercel.app/payment-{status}");
            }

            return BadRequest("Loại thanh toán không hợp lệ.");
        }
        [HttpGet("BuyListing/{listingId}")]
        public async Task<IActionResult> BuyListing(Guid listingId)
        {
            var result = await _listingService.BuyListing(listingId, HttpContext);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }
    }
}
