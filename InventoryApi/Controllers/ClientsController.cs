using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsServices _clientsServices;
        public ClientsController(IClientsServices clientsServices)
        {
            _clientsServices = clientsServices;
        }

        [HttpGet("client-info/{clientId}")]
        public async Task<IActionResult> GetResult(int clientId)
        {
            var result = await _clientsServices.GetInfoClient(clientId);
            return Ok(result);
        }
        [HttpGet("all-client")]
        public async Task<IActionResult> AllClient()
        {
            var result = await _clientsServices.GetAllClient();
            return Ok(result);
        }
        [HttpPost("add-client")]
        public async Task<IActionResult> AddClient(string name,int passport,DateOnly birth)
        {
            var result = await _clientsServices.AddClient(name, passport, birth);
            return Ok(result);
        }
        [HttpPut("update-client/{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Clients clients)
        {
            await _clientsServices.UpdateClient(id, clients.FullName, clients.Passport);
            return Ok();
        }
        [HttpDelete("delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientsServices.DeleteClient(id);
            return Ok(result);
        }
        [HttpGet("client-tran")]
        public async Task<IActionResult> GetWithTran()
        {
            var result = await _clientsServices.GetListTran();

            return Ok(result);
        }
        [HttpGet("tran")]
        public async Task<IActionResult> GetResult()
        {
            var result = await _clientsServices.Get();

            return Ok(result);
        }
    }
}

