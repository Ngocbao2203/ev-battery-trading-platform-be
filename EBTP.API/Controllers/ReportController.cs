using EBTP.Service.DTOs.Report;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _reportService.GetAllReports(pageIndex, pageSize);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var result = await _reportService.GetById(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetReportsByUserId(Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _reportService.GetReportByUserId(userId, pageIndex, pageSize);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("listing/{listingId:guid}")]
        public async Task<IActionResult> GetReportsByListingId(Guid listingId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _reportService.GetReportsByListingId(listingId, pageIndex, pageSize);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public async Task<IActionResult> SendReport([FromForm] CreateReportDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Error = 1, Message = "Dữ liệu không hợp lệ." });

            var result = await _reportService.SendReport(dto);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            var result = await _reportService.DeleteReport(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
