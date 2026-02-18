using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsServices _transactionsServices;
 
        public TransactionsController(ITransactionsServices transactionsServices)
        {
            _transactionsServices = transactionsServices;

        }
        [HttpGet("all-transactions")]
        public async Task<IActionResult> Get()
        {
            var transactions = await _transactionsServices.GetList();

            return Ok(transactions);
        }
        [HttpPost("add-transaction")]
        public async Task<IActionResult> Post([FromBody] TransactionCreateDto dto)
        {
            var transaction = await _transactionsServices.Add(dto);

            return Ok(transaction);
        }
    }
}
