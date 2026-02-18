using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkersServices _workersServices;
        public WorkersController(IWorkersServices workersServices) 
        {
            _workersServices = workersServices;
        }
        [HttpGet("all-workers")]
        public async Task<IActionResult> Get()
        {
            var worker = await _workersServices.GetInfoWorker();

            return Ok(worker);
        }
        [HttpGet("all-with")]
        public async Task<IActionResult> GetWith()
        {
            var workers = await _workersServices.GetWithTran();

            return Ok(workers);
        }
        [HttpGet("worker/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var worker = await _workersServices.GetInfoWorker(id);
            return Ok(worker);
        }
        [HttpPost("add-worker")]
        public async Task<IActionResult> Post([FromBody] Workers workers)
        {
            await _workersServices.AddNewWorker(workers.FullName, workers.Post);
            return Ok();
        }
        [HttpPut("uptade-worker/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Workers workers)
        {
            await _workersServices.UpdateWorker(id, workers.FullName, workers.Post);
            return Ok();
        }
        [HttpDelete("delete-worker/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _workersServices.DeleteWorker(id);
            return Ok();
        }
        
    }
}
