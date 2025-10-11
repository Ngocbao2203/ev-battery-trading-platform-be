using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.Brand;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _brandService.GetAllAsync();
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetById/id")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _brandService.GetByIdAsync(id);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBrandDTO dto)
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
            var result = await _brandService.CreateAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateBrandDTO dto)
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
            var result = await _brandService.UpdateAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("Delete/id")]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            var result = await _brandService.DeleteAsync(id);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
