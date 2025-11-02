using EBTP.Service.DTOs.Favourite;
using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpGet("GetFavourites/{userId}")]
        public async Task<IActionResult> GetCraftVillageById(Guid userId)
        {
            var result = await _favouriteService.GetFavourites(userId);
            if (result.Error != 0)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("CheckFavourite")]
        public async Task<IActionResult> CheckFavourite([FromQuery] Guid userId, [FromQuery] Guid listingId)
        {
            var result = await _favouriteService.CheckFavourite(userId, listingId);
            return Ok(new { isFavorited = result.Data });
        }

        [HttpPost("AddFavourite")]
        public async Task<IActionResult> AddFavourite([FromBody] CreateFavouriteDTO favourite)
        {
            var result = await _favouriteService.AddFavourite(favourite);
            return Ok(result);
        }

        [HttpDelete("DeleteFavourite")]
        public async Task<IActionResult> DeleteFavourite([FromQuery] Guid userId, [FromQuery] Guid listingId)
        {
            var result = await _favouriteService.DeleteFavouriteByUserAndListing(userId, listingId);
            return Ok(result);
        }
    }
}
