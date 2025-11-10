using EBTP.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBTP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("GetTransactionsByUserId/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserId(Guid userId, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _transactionService.GetTransactionsByUserId(userId, pageIndex, pageSize);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
